using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtidRegister.Areas.Admin.Controllers
{
    /// <summary>
    /// Registration Finallization Controller
    /// </summary>
    [Area("Admin")]   
    [Authorize(Roles = "Admin")]
    public class FinalizeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult StartAutoScheduling()
        {
            return View();
        }

        public IActionResult IsAutoSchedulingDone()
        {
            return Json(false);
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult SendNotifications()
        {
            return View();
        }
    }
}