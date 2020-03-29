using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtidRegister.Areas.Admin.Models;
using AtidRegister.Models;
using AtidRegister.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtidRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FAQsController : Controller
    {
        private IFAQsService _svc;
        public FAQsController(IFAQsService svc)
        {
            _svc = svc;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _svc.GetFAQuestionsAsync());
        }
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreareFAQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                await _svc.AddFAQuestion(model.Question, model.Answer);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var faq = await _svc.FindByIdAsync(id);
            return View(new EditFAQuestionViewModel() { Id = faq.Id, Answer = faq.Answer, Question = faq.Question });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditFAQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var q = await _svc.FindByIdAsync(model.Id);
                if (q != null) {
                    q.Question = model.Question;
                    q.Answer = model.Answer;
                    await _svc.UpdateFAQuestion(q);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, string s) => View(await _svc.FindByIdAsync(id));
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.RemoveFAQuestion(id);
            return RedirectToAction("Index");
        }
    }
}