using AtidRegister.Data;
using AtidRegister.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Services.Conference
{
    public interface IFinalizingService
    {
        /// <summary>
        /// Returns whether any kind of scheduling was done.
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsAnySchedualingDone();
    }
    public class FinalizingService : IFinalizingService
    {
        private readonly AppDbContext _ctx;

        public FinalizingService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> IsAnySchedualingDone()
        {
            return await _ctx.UserSchedules.AnyAsync();
        }

        public async Task AutoScheduling()
        {
            var allSelections = (await _ctx.ContentUsers.ToListAsync());
            allSelections.Shuffle(); // shuffle selection to promise randomization
            var contents = (await _ctx.Contents.ToListAsync()).OrderBy(c => c.SchedulingPriority);
            var schedules = new List<UserSchedule>();
            var regUsersId = _ctx.ContentUsers.GroupBy(cu => cu.UserId).Select(gcu => gcu.Key);
            // for each register record, add by prioritization of scheduling
            // one time for time strip.
            foreach (var content in contents)
            {
                var contentSelections = allSelections.Where(s => s.ContentId == content.Id).ToList();
                for (int i = 0; i < content.SpaceLimitation && contentSelections.Count > 0; i++)
                {
                    var currSelection = contentSelections[i];
                    var userId = currSelection.UserId;
                    schedules.Add(new Models.UserSchedule()
                    {
                        ContentId = content.Id,
                        UserId = userId
                    });
                    // remove all in timestrip of user
                    allSelections.RemoveAll(s => s.Content.TimeStripId == currSelection.Content.TimeStripId && s.UserId == userId);
                    contentSelections.RemoveAt(0); // remove first
                }
            }
            var notRegistered = (await _ctx.Users.ToListAsync()).Where(u => !regUsersId.Any(ruid => ruid == u.Id)).ToList();
            var timeStrips = _ctx.TimeStrips;
            var notRegisteredTimeStrips = notRegistered.Select(u => timeStrips.Select(ts => new SimpleUserTimeStrip() { TimeStripId = ts.Id, UserId = u.Id }));
            notRegistered.Shuffle();
            // now, fill up by priorities all left students
            foreach (var content in contents)
            {
                // find all scheduled. if more schedules then limitation, skip.
                var selections = schedules.Where(s => s.ContentId == content.Id);
                if (content.SpaceLimitation >= selections.Count())
                    continue;

            }
            _ctx.UserSchedules.AddRange(schedules);
            await _ctx.SaveChangesAsync();
        }
    }
    static class FinalizingExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            Random rng = new Random(219643);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    class SimpleUserTimeStrip
    {
        public int TimeStripId { get; set; }
        public string UserId { get; set; }
    }
}
