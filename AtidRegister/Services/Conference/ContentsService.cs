using AtidRegister.Configuration;
using AtidRegister.Data;
using AtidRegister.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Services.Conference
{
    public interface IContentsService
    {
        /// <summary>
        /// Adds a content
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="people"></param>
        /// <param name="type"></param>
        /// <param name="image">base64 image</param>
        /// <param name="timeStripId"></param>
        /// <returns></returns>
        public Task<int> AddContentAsync(string title, string description, IEnumerable<Person> people, ContentType type, string image, int timeStripId);
        /// <summary>
        /// Removes a content by it's PK
        /// </summary>
        /// <param name="id">the PK</param>
        public Task RemoveContentAsync(int id);
        /// <summary>
        /// Returns all the contents
        /// </summary>
        public Task<List<Content>> GetContentsAsync();
        /// <summary>
        /// updates a content 
        /// </summary>
        /// <param name="updatedContent">the updates content</param>
        /// <param name="people">the new people should be assigned to it</param>
        public Task UpdateContentAsync(Content updatedContent, List<Person> people);
        /// <summary>
        /// Returns a content by it's PK
        /// </summary>
        /// <param name="id">the PK</param>
        public Task<Content> FindContentByIdAsync(int id);
        /// <summary>
        /// Returns a content type by it's PK
        /// </summary>
        /// <param name="typeId">the PK</param>
        /// <returns></returns>
        public Task<ContentType> FindContentTypeByIdAsync(int typeId);
        /// <summary>
        /// Returns all the content types
        /// </summary>
        /// <returns></returns>
        public Task<List<ContentType>> GetContentTypesAsync();
        /// <summary>
        /// updates a content type (name)
        /// </summary>
        /// <param name="id">the content type PK</param>
        /// <param name="name">the content type new name</param>
        /// <returns></returns>
        public Task UpdateContentTypeAsync(int id, string name);
        /// <summary>
        /// Removes a content types by it's PK
        /// </summary>
        /// <param name="id">the PK</param>
        public Task RemoveContentTypeAsync(int id);
        /// <summary>
        /// Adds a content type by it's name and returns it's id
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>the new content type id</returns>
        public Task<int> AddContentTypeAsync(string name);
        /// <summary>
        /// add a time strip by it's start and end
        /// </summary>
        /// <param name="start">the start hour</param>
        /// <param name="end">the end hour</param>
        /// <returns></returns>
        public Task AddTimeStripAsync(TimeSpan start, TimeSpan end);
        /// <summary>
        /// removes a time strip by it's PK
        /// </summary>
        /// <param name="id">the PK</param>
        /// <returns></returns>
        public Task RemoveTimeStripAsync(int id);
        /// <summary>
        /// updates a time strip
        /// </summary>
        /// <param name="timeStrip">the time strip to update</param>
        /// <returns></returns>
        public Task UpdateTimeStripAsync(TimeStrip timeStrip);
        /// <summary>
        /// returns time strip by it's PK
        /// </summary>
        /// <param name="id">the PK</param>
        /// <returns></returns>
        public Task<TimeStrip> FindTimeStripByIdAsync(int id);
        /// <summary>
        /// returns all the time strips
        /// </summary>
        /// <returns></returns>
        public Task<List<TimeStrip>> GetTimeStripsAsync();
        /// <summary>
        /// returns whether a time strip is valid by the start and end given
        /// </summary>
        /// <returns></returns>
        public Task<bool> CheckTimeStripAsync(TimeSpan start, TimeSpan end);
    }
    public class ContentsService : IContentsService
    {
        private readonly AppDbContext _ctx;
        private readonly AppConfig _config;

        public ContentsService(AppDbContext ctx,
                            IOptions<AppConfig> config)
        {
            _ctx = ctx;
            _config = config.Value;
        }
        /*
        public async Task<int> AddContentAsync(string title, string description, IEnumerable<Person> people, ContentType type)
        {
            return await AddContentAsync(title, description, people, type, _config.DefaultContentImage);
        }*/

        public async Task<int> AddContentAsync(string title, string description, IEnumerable<Person> people, ContentType type, string imageFile, int timeStripId)
        {
            // add content:
            var content = new Content() { Title = title, Description = description, ImageFile = imageFile, TypeId = type.Id, TimeStripId = timeStripId };
            _ctx.Contents.Add(content);
            await _ctx.SaveChangesAsync();
            // add people:
            var peopleContentsList = new List<ContentPerson>();
            foreach (var person in people)
            {
                peopleContentsList.Add(new ContentPerson() { ContentId = content.Id, PersonId = person.Id });
            }
            // add to db
            await _ctx.ContentPeople.AddRangeAsync(peopleContentsList);
            await _ctx.SaveChangesAsync();
            return content.Id;
        }

        public async Task<int> AddPersonAsync(string name, string jobTitle)
        {
            return await AddPersonAsync(name, jobTitle, _config.DefaultPersonImage);
        }

        public async Task<int> AddPersonAsync(string name, string jobTitle, string imageFile)
        {
            var person = new Person() { FullName = name, JobTitle = jobTitle, ImageFile = imageFile };
            _ctx.People.Add(person);
            await _ctx.SaveChangesAsync();
            return person.Id;
        }

        public async Task RemoveContentAsync(int id)
        {
            var content = await _ctx.Contents.FirstOrDefaultAsync(p => p.Id == id);
            _ctx.Remove(content);
            await _ctx.SaveChangesAsync();
        }

        public async Task RemovePersonAsync(int id)
        {
            var person = await _ctx.People.FirstOrDefaultAsync(p => p.Id == id);
            _ctx.Remove(person);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateContentAsync(Content updatedContent, List<Person> people)
        {
            var content = await _ctx.Contents.FirstOrDefaultAsync(c => c.Id == updatedContent.Id);
            content.Description = updatedContent.Description;
            content.Title = updatedContent.Title;
            content.ImageFile = updatedContent.ImageFile;
            _ctx.Contents.Update(updatedContent);

            var allPeople = await _ctx.People.ToListAsync();
            var contentPeople = await _ctx.ContentPeople.Where(cp => cp.ContentId == content.Id).ToListAsync();
            // for each person
            foreach (var person in allPeople)
            {
                bool isPersonInDbForContent = contentPeople.Any(cp => cp.PersonId == person.Id);
                // if it's in the given list
                if (people.Any(p => p.Id == person.Id))
                {
                    // and not in db
                    if (!isPersonInDbForContent)
                    {
                        // add it to db
                        await AddPersonToContentAsync(person.Id, content.Id);
                    }
                }
                else
                {
                    // if not in list
                    // but in db
                    if (isPersonInDbForContent)
                    {
                        // remove it
                        _ctx.ContentPeople.Remove(contentPeople.FirstOrDefault(cp => cp.PersonId == person.Id));
                    }
                }
            }
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person updatedPerson)
        {
            _ctx.People.Update(updatedPerson);
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<Content>> GetContentsAsync()
        {
            return await _ctx.Contents.Include(c => c.TimeStrip).Include(c => c.Type).Include(c => c.ContentPeople).ThenInclude(c => c.Person).ToListAsync();
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            return await _ctx.People.ToListAsync();
        }

        public async Task<Person> FindPersonByIdAsync(int id)
        {
            return await _ctx.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Content> FindContentByIdAsync(int id)
        {
            return await _ctx.Contents.Include(p => p.TimeStrip).Include(p => p.Type).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPersonToContentAsync(int personId, int contentId)
        {
            var contentPerson = await _ctx.ContentPeople.FirstOrDefaultAsync(cp => cp.ContentId == contentId && cp.PersonId == personId);
            if (contentPerson == null)
            {
                contentPerson = new ContentPerson() { ContentId = contentId, PersonId = personId };
                _ctx.ContentPeople.Add(contentPerson);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task RemovePersonFromContentAsync(int personId, int contentId)
        {
            var contentPerson = await _ctx.ContentPeople.FirstOrDefaultAsync(cp => cp.ContentId == contentId && cp.PersonId == personId);
            if (contentPerson != null)
            {
                _ctx.ContentPeople.Remove(contentPerson);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserSelectedContent(AppUser user, int contentId)
        {
            return (await _ctx.ContentUsers.FirstOrDefaultAsync(cu => cu.ContentId == contentId && cu.UserId == user.Id)) != null;
        }

        public async Task<ContentType> FindContentTypeByIdAsync(int typeId)
        {
            return await _ctx.ContentTypes.FirstOrDefaultAsync(ct => ct.Id == typeId);
        }

        public async Task<List<ContentType>> GetContentTypesAsync()
        {
            return await _ctx.ContentTypes.Include(ct => ct.Contents).ToListAsync();
        }

        public async Task UpdateContentTypeAsync(int id, string name)
        {
            var ct = await _ctx.ContentTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (ct != null && name != ct.Name)
            {
                ct.Name = name;
                _ctx.ContentTypes.Update(ct);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task RemoveContentTypeAsync(int id)
        {
            var ct = await _ctx.ContentTypes.FirstOrDefaultAsync(ct => ct.Id == id);
            if (ct != null)
            {
                _ctx.ContentTypes.Remove(ct);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<int> AddContentTypeAsync(string name)
        {
            var ct = new ContentType() { Name = name };
            _ctx.ContentTypes.Add(ct);
            await _ctx.SaveChangesAsync();
            return ct.Id;
        }

        public async Task<bool> IsPersonInContentAsync(Content content, Person person)
        {
            return await _ctx.ContentPeople.AnyAsync(cp => cp.PersonId == person.Id && cp.ContentId == content.Id);
        }

        public async Task<List<Content>> GetUserSelectedContentsAsync(AppUser user)
        {
            return await _ctx.ContentUsers.Include(cu => cu.Content).ThenInclude(c => c.ContentPeople).ThenInclude(cp => cp.Person)
                .Where(cu => cu.UserId == user.Id).Select(cu => cu.Content).ToListAsync();
        }

        public async Task<int> GetNumberOfRegisteredUsers()
        {
            return await _ctx.ContentUsers.GroupBy(cu => cu.UserId).CountAsync();
        }

        public async Task<bool> IsUserSelectedAnyContent(AppUser user)
        {
            return await _ctx.ContentUsers.AnyAsync(cu => cu.UserId == user.Id);
        }

        public async Task AddTimeStripAsync(TimeSpan start, TimeSpan end)
        {
            await _ctx.TimeStrips.AddAsync(new TimeStrip() { StartTime = start, EndTime = end });
            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveTimeStripAsync(int id)
        {
            _ctx.TimeStrips.Remove(await _ctx.TimeStrips.FirstOrDefaultAsync(ts => ts.Id == id));
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateTimeStripAsync(TimeStrip timeStrip)
        {
            _ctx.TimeStrips.Update(timeStrip);
            await _ctx.SaveChangesAsync();
        }

        public async Task<TimeStrip> FindTimeStripByIdAsync(int id)
        {
            return await _ctx.TimeStrips.FirstOrDefaultAsync(ts => ts.Id == id);
        }

        public async Task<List<TimeStrip>> GetTimeStripsAsync()
        {
            return await _ctx.TimeStrips.ToListAsync();
        }

        public async Task<bool> CheckTimeStripAsync(TimeSpan start, TimeSpan end)
        {
            return start.CompareTo(end) >= 0 && (await GetTimeStripsAsync()).All(ts => ts.StartTime.CompareTo(end) < 0 || ts.EndTime.CompareTo(start) > 0);
        }
    }
}
