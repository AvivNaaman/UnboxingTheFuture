using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtidRegister.Areas.Admin.Models;
using AtidRegister.Services;
using AtidRegister.Services.Conference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtidRegister.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin home
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IStudentService _studentService;

        public HomeController(IAdminService adminService, IStudentService studentService)
        {
            _adminService = adminService;
            _studentService = studentService;
        }
        /// <summary>
        /// Admin home dashboard
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var registered = await _studentService.GetRegisteredStudentsCountAsync();
            var registerable = await _studentService.GetStudentsCountAsync();
            var UnregisteredUsers = registerable - registered;
            var contentsWithRegCounts = await _studentService.GetContentsWithRegisteredCountAsync();
            var model = new AdminIndexViewModel()
            {
                UnregisteredUsers = UnregisteredUsers,
                UsersRegistered = registered,
                ContentsWithRegCounts = contentsWithRegCounts,
                HasPhoneNumber = await _studentService.HasPhoneNumberCountAsync()
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // if authenticated, prevent from logging in again
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _adminService.LoginAsync(model.UserName, model.Password))
                    return RedirectToAction("Index", new { controller = "Home" });
                ModelState.AddModelError("", "Authetication Failed.");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> NotRegisteredReport()
        {
            return Content(await _studentService.GetNotRegisterdAsCsvAsync());  
        }
        [HttpGet]
        public async Task<IActionResult> FullReport()
        {
            return Content(await _studentService.GenerateFullReport());
        }
    }
}