using AtidRegister.Data;
using AtidRegister.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Services
{
    public interface IFAQsService
    {
        public  Task<List<FAQuestion>> GetFAQuestionsAsync();
        public Task AddFAQuestion(string q, string a);
        public Task RemoveFAQuestion(int id);
        public Task UpdateFAQuestion(FAQuestion FAQuestion);
        public Task<FAQuestion> FindByIdAsync(int id);
    }
    public class FAQsService : IFAQsService
    {
        private AppDbContext _ctx;
        public FAQsService(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddFAQuestion(string q, string a)
        {
            await _ctx.FAQuestions.AddAsync(new FAQuestion() { Question = q, Answer = a });
            await _ctx.SaveChangesAsync();
        }

        public async Task<FAQuestion> FindByIdAsync(int id)
        {
            return await _ctx.FAQuestions.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<FAQuestion>> GetFAQuestionsAsync()
        {
            return await _ctx.FAQuestions.ToListAsync();
        }

        public async Task RemoveFAQuestion(int id)
        {
            var q = await _ctx.FAQuestions.FirstOrDefaultAsync(faq => faq.Id == id);
            _ctx.FAQuestions.Remove(q);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateFAQuestion(FAQuestion FAQuestion)
        {
            _ctx.FAQuestions.Update(FAQuestion);
            await _ctx.SaveChangesAsync();
        }
    }
}
