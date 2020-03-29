using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtidRegister.Areas.Admin.Models;
using AtidRegister.Models;
using AtidRegister.Services;
using AtidRegister.Services.Conference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtidRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles= "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userSvc;
        private readonly IStudentService _studentService;

        public UsersController(IUserService userSvc, IStudentService studentService)
        {
            _userSvc = userSvc;
            _studentService = studentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new UsersIndexViewModel() { Students = await GetBaseNonAdmins(), Admins = await GetBaseAdmins() };
            return View(model);
        }

        private async Task<List<BaseStudentsVM>> GetBaseNonAdmins()
        {
            var users = (await _studentService.GetStudentsAsync());
            var result = new List<BaseStudentsVM>();
            foreach (var user in users)
            {
                result.Add(new BaseStudentsVM()
                {
                    UserName = user.UserName,
                    DidChose = await _studentService.IsStudentRegisteredAsync(user),
                    Grade = user.Grade
                });
            }
            return result;
        }
        private async Task<List<BaseAdminVM>> GetBaseAdmins()
        {
            List<AppUser> admins = await _userSvc.GetAdminsAsync();
            var result = new List<BaseAdminVM>();
            foreach (var admin in admins)
            {
                result.Add(new BaseAdminVM()
                {
                    Email = admin.Email,
                    UserName = admin.UserName
                });
            }
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> BulkCreate()
        {
            return View(new BulkCreateUsersViewModel() { Classes = await _studentService.GetClassesAsync() });
        }
        [HttpPost]
        public async Task<IActionResult> BulkCreate(BulkCreateUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await _studentService.CreateBulkAsync(model.csvFile.OpenReadStream(), model.GradeId);
                if (result)
                    return RedirectToAction("Index");
                else ModelState.AddModelError("csvFile", "an Error occured during the update of the students list. Check the log.");
            }
            model.Classes = await _studentService.GetClassesAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CreateUserViewModel() { Classes = await _studentService.GetClassesAsync() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model) 
        {
            if (ModelState.IsValid)
            {
                if (model.IsAdmin)
                    await _userSvc.CreateAdminAsync(model.Name, model.Password, model.Email);
                else
                    await _studentService.CreateAsync(model.Name, model.GradeId);
            }
            model.Classes = await _studentService.GetClassesAsync();
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdminTemp(string user, string pwd, string email)
        {
            await _userSvc.CreateAdminAsync(user, pwd, email);
            return new EmptyResult();
        }
    }
}