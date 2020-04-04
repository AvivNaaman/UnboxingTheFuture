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
    /// Time strips controller (just basic read/write)
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TimeController : Controller
    {
        private readonly IContentsService _contentsService;

        public TimeController(IContentsService contentsService)
        {
            _contentsService = contentsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _contentsService.GetTimeStripsAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _contentsService.FindContentByIdAsync(id));
        }
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateTimeStripViewModel model)
        {
            if (ModelState.IsValid)
            {
                var start = new TimeSpan(model.StartHour, model.StartMinute, 0);
                var end = new TimeSpan(model.EndHour, model.EndMinute, 0);
                if (await _contentsService.CheckTimeStripAsync(start, end)) {
                    ModelState.AddModelError("", "שעת התחלה חייבת להיות לפני שעת סיום, וגם רצועת הזמן לעולם לא תיחתך עם אחת הקיימות.");
                    return View(model);
                }
                await _contentsService.AddTimeStripAsync(start, end);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _contentsService.FindContentByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _contentsService.FindContentByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string s)
        {
            await _contentsService.RemoveTimeStripAsync(id);
            return RedirectToAction("Index");
        }
    }
}