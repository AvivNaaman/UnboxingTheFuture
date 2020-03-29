using AtidRegister.Configuration;
using AtidRegister.Data;
using AtidRegister.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AtidRegister.Services.Conference
{
    public interface IPeopleService
    {

        public Task AddPersonToContentAsync(int personId, int contentId);
        public Task<int> AddPersonAsync(string name, string jobTitle);
        public Task<int> AddAsync(string name, string jobTitle, string imageFile);
        public Task RemoveAsync(int id);
        public Task<List<Person>> GetAsync();
        public Task UpdateAsync(Person updatedPerson);
        public Task<Person> FindByIdAsync(int id);
        public Task<bool> IsInContentAsync(Person person, Content content);
    }
    public class PeopleService : IPeopleService
    {
        private readonly AppConfig _config;
        private readonly AppDbContext _ctx;

        public PeopleService(AppDbContext ctx, IOptions<AppConfig> config)
        {
            _config = config.Value;
            _ctx = ctx;
        }
        public async Task<int> AddPersonAsync(string name, string jobTitle)
        {
            return await AddAsync(name, jobTitle, _config.DefaultPersonImage);
        }

        public async Task<int> AddAsync(string name, string jobTitle, string imageFile)
        {
            var person = new Person() { FullName = name, JobTitle = jobTitle, ImageFile = imageFile };
            _ctx.People.Add(person);
            await _ctx.SaveChangesAsync();
            return person.Id;
        }
        public async Task RemoveAsync(int id)
        {
            var person = await _ctx.People.FirstOrDefaultAsync(p => p.Id == id);
            _ctx.Remove(person);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person updatedPerson)
        {
            _ctx.People.Update(updatedPerson);
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<Person>> GetAsync()
        {
            return await _ctx.People.ToListAsync();
        }

        public async Task<Person> FindByIdAsync(int id)
        {
            return await _ctx.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task AddPersonToContentAsync(int personId, int contentId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInContentAsync(Person person, Content content)
        {
            return await _ctx.ContentPeople.AnyAsync(cp => cp.ContentId == content.Id && cp.PersonId == person.Id);
        }
    }
}
