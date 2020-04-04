using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AtidRegister.Models;
using Microsoft.AspNetCore.Authorization;
using AtidRegister.Models.ViewModels;
using AtidRegister.Services;
using Microsoft.Extensions.Options;
using AtidRegister.Configuration;
using AtidRegister.Services.Conference;

namespace AtidRegister.Controllers
{
    [Authorize(Roles = "Student")] // student-only page
    public class HomeController : Controller
    {
        #region Services
        private readonly IContentsService _contentsService;
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly IFAQsService _faqService;
        private readonly AppConfig _config;
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminService _adminService;
        #endregion

        // CTOR
        public HomeController(IContentsService contentsService, IUserService userService, ILogger<HomeController> logger,
            IStudentService studentService, IFAQsService faqsService, IOptions<AppConfig> config, IAdminService adminService)
        {
            _contentsService = contentsService;
            _userService = userService;
            _studentService = studentService;
            _faqService = faqsService;
            _config = config.Value;
            _logger = logger;
            _adminService = adminService;
        }

        #region Register
        /// <summary>
        /// User Registration Home
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync();
            return View(new IndexViewModel()
            {
                Contents = (await GetContentChecksOfUser(user)),
                TimeStrips = (await _contentsService.GetTimeStripsAsync()).OrderBy(ts => ts.StartTime).ToList(),
                UserName = user.FriendlyName,
                Priorities = await _studentService.GetUserPrioritiesAsync(await _userService.GetCurrentUserAsync())
            });
        }
        /// <summary>
        /// Returns the currently selected contents by the user
        /// </summary>
        /// <param name="user">The user to look for it's contents</param>
        /// <returns>List of ContentCheck by whether the content are selected or not</returns>
        private async Task<List<ContentCheck>> GetContentChecksOfUser(AppUser user)
        {
            var result = new List<ContentCheck>();
            var selectedContents = await _studentService.GetUserSelectedContentsAsync(user);
            foreach (var content in await _contentsService.GetContentsAsync())
            {
                result.Add(new ContentCheck()
                {
                    Content = content,
                    isChecked = selectedContents.Any(c => c.Id == content.Id)
                });
            }
            return result.Shuffle();
        }
        /// <summary>
        /// User Registration POST
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            var user = await _userService.GetCurrentUserAsync();
            var timeStrips = await _contentsService.GetTimeStripsAsync();
            string[] prioritiesSplitted = model.Priorities.Split('$');
            if (prioritiesSplitted.Length == timeStrips.Count)
            {
                List<Content> contentsToAdd = new List<Content>();
                if (prioritiesSplitted.All(ps => ps.Split('.').Count() >= _config.MinPrioritiesPerTimeline+1))
                {
                    var res = await _studentService.RegisterByStringAsync(user, model.Priorities);
                    return RedirectToAction("Success");
                }
            }
            var allContents = await _contentsService.GetContentsAsync();
            model.UserName = user.UserName;
            model.TimeStrips = timeStrips;
            var newContents = GetContentChecksWithPreserve(model.Contents, allContents);
            model.Contents = newContents;
            ModelState.AddModelError("", "נא לבחור "+_config.MinPrioritiesPerTimeline+" לפחות בכל קבוצה.");
            return View(model);
        }
        /// <summary>
        /// Returns the selected contents from the model with preservation of checked things.
        /// </summary>
        /// <param name="contents">The List of the contentChecks from the model</param>
        /// <param name="allContents">The List of all the contents</param>
        /// <returns>List of ContentCheck by whether the content are selected or not</returns>
        private List<ContentCheck> GetContentChecksWithPreserve(List<ContentCheck> contents, List<Content> allContents)
        {
            foreach (var c in contents)
            {
                c.Content = allContents.FirstOrDefault(co => co.Id == c.Content.Id);
            }
            return contents;
        }
        /// <summary>
        /// User Successful Registration. Redirects to PhoneNumber if needed.
        /// </summary>
        public async Task<IActionResult> Success()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (String.IsNullOrEmpty(user.PhoneNumber))
            {
                // if no phone number, redirect to phone number:
                return RedirectToAction("UpdatePhone", new { user.Id });
            }
            var groupings = await _studentService.GetStudentUserContetnsByTimeStripsAsync(user);
            var model = await _contentsService.GetTimeStripsAsync();
            // log user out
            await _userService.Logout();
            return View(model);
        }
        #endregion Register
        /// <summary>
        /// Welcome screen (basic instructions)
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetStarted()
        {
            var student = await _userService.GetCurrentUserAsync();
            if (await _studentService.IsStudentRegisteredAsync(student))
                return Redirect("~/");
            else return View();
        }

        #region UpdatePhone
        /// <summary>
        /// User Phone Update Page
        /// </summary>
        /// <param name="id">The PK of the user</param>
        [HttpGet]
        public IActionResult UpdatePhone(string id) => View(new UpdatePhoneViewModel() { UserId = id });
        /// <summary>
        /// User Phone update POST
        /// </summary>
        /// <param name="m">The model containing the relevant info</param>
        [HttpPost]
        public async Task<IActionResult> UpdatePhone(UpdatePhoneViewModel m)
        {
            if (ModelState.IsValid)
            {
                var student = await _studentService.FindByIdAsync(m.UserId);
                if (student != null)
                {
                    int finalNumber;
                    if (int.TryParse(m.PhoneNumber, out finalNumber))
                    {
                        if (await _studentService.ChangeStudentPhoneNumberAsync(student, finalNumber))
                        {
                            return RedirectToAction("Success");
                        }
                        ModelState.AddModelError("PhoneNumber","נא להזין מספר טלפון חוקי.");
                    }
                    else
                    {
                        ModelState.AddModelError("PhoneNumber", "נא להזין מספר טלפון חוקי.");
                    }
                }
            }
            return View(m);
        }
        #endregion

        #region PublicActions
        /// <summary>
        /// FAQs Page
        /// </summary>
        [AllowAnonymous]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> FAQs() => View(await _faqService.GetFAQuestionsAsync());
        /// <summary>
        /// Privacy Page
        /// </summary>
        [AllowAnonymous]
        [ResponseCache(Duration = 3600)]
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Error Page (500.Xs)
        /// </summary>
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (await _adminService.IsCurrentUserAdminAsync())
                {
                    return RedirectToAction("Index", new { controler = "Home", area = "Admin" });
                }
                return RedirectToAction("Index", new { controller = "Home" });
            }
            // send model with grades
            var model = new LoginViewModel() { Classes = await _studentService.GetClassesAsync() };
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // log user in
                await _studentService.LoginAsync(model.UserId);
                // redirect to registration
                if (!(await _studentService.IsStudentRegisteredAsync(await _studentService.FindByIdAsync(model.UserId))))
                {
                    return RedirectToAction("GetStarted");
                }
                return RedirectToAction("Index");
            }
            // if invalid, refill classes and resend
            _logger.LogError("Binding Failed for user id. resending new model.");
            model.Classes = await _studentService.GetClassesAsync();
            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGrades(int id)
        {
            var g = (await _studentService.GetGradesAsync(id)).OrderByDescending(g => g.Id);
            return Json(g);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudents(int id)
        {
            var g = (await _studentService.GetStudentsAsync(id)).OrderBy(g => g.UserName).Select(u => new { UserName = u.UserName, GradeId = u.GradeId, Id = u.Id });
            return Json(g);
        }
        #endregion
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // log user out.
            await _userService.Logout();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
    /// <summary>
    /// Extension Methods Required by class HomeController
    /// </summary>
    public static class HomeExtensions
    {
        private static readonly Random rng = new Random();
        /// <summary>
        /// Shuffles a <typeparamref name="T"/> List
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="list">The list to shuffle</param>
        /// <returns>the shuffled list</returns>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
