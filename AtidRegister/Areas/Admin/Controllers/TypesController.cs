using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtidRegister.Areas.Admin.Models;
using AtidRegister.Services.Conference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtidRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TypesController : Controller
    {
        private readonly IContentsService _contentsService;

        public TypesController(IContentsService contentsService)
        {
            _contentsService = contentsService;
        }
        #region Types
        [HttpGet]
        public async Task<IActionResult> Index() => View(await _contentsService.GetContentTypesAsync());
        [HttpGet]
        public IActionResult Create() => View();
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int a)
        {
            var type = await _contentsService.FindContentTypeByIdAsync(id);
            if (type != null)
            {
                return View(new TypeViewModel() { Name = type.Name, Id = type.Id });
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var type = await _contentsService.FindContentTypeByIdAsync(id);
            if (type != null)
            {
                return View(new TypeViewModel() { Name = type.Name, Id = type.Id });
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = await _contentsService.AddContentTypeAsync(model.Name);
                return RedirectToAction("TypeDetails", new { id = id });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            await _contentsService.RemoveContentTypeAsync(Id);
            return RedirectToAction("ContentTypes");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _contentsService.UpdateContentTypeAsync(model.Id, model.Name);
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }
        #endregion
    }
}