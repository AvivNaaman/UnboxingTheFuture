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
        /// <summary>
        /// Adds a person to content
        /// </summary>
        /// <param name="personId">the person PK</param>
        /// <param name="contentId">the content PK</param>
        /// <returns></returns>
        public Task AddPersonToContentAsync(int personId, int contentId);
        /// <summary>
        /// Adds a person by it's name & job title
        /// </summary>
        /// <param name="name">the person's name</param>
        /// <param name="jobTitle">it's job title</param>
        /// <returns></returns>
        public Task<int> AddAsync(string name, string jobTitle);
        /// <summary>
        /// adds a person by it's job title, name & image
        /// </summary>
        /// <param name="name">the person's name</param>
        /// <param name="jobTitle">the person's job title</param>
        /// <param name="imageFile">the person's image, base64</param>
        /// <returns></returns>
        public Task<int> AddAsync(string name, string jobTitle, string imageFile);
        /// <summary>
        /// Removes a person by it's PK
        /// </summary>
        /// <param name="id">the PK</param>
        /// <returns></returns>
        public Task RemoveAsync(int id);
        /// <summary>
        /// Returns all the people
        /// </summary>
        /// <returns></returns>
        public Task<List<Person>> GetAsync();
        /// <summary>
        /// updates a person 
        /// </summary>
        /// <param name="updatedPerson">the updates person</param>
        /// <returns></returns>
        public Task UpdateAsync(Person updatedPerson);
        /// <summary>
        /// returns a person by it's PK
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Person> FindByIdAsync(int id);
        /// <summary>
        /// Returns whehther the person is in the content
        /// </summary>
        /// <param name="person">the person</param>
        /// <param name="content">the content</param>
        /// <returns></returns>
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
        public async Task<int> AddAsync(string name, string jobTitle)
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
