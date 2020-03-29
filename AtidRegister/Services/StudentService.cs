using AtidRegister.Data;
using AtidRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtidRegister.Services
{
    public interface IStudentService
    {
        /// <summary>
        /// logs in the students asynchoronosly.
        /// </summary>
        /// <param name="userId">The PK of the user</param>
        public Task<bool> LoginAsync(string userId);
        #region Getters
        /// <summary>
        /// Returns Classes async.
        /// </summary>
        /// <returns>List of classes async</returns>
        Task<List<Class>> GetClassesAsync();
        /// <summary>
        /// Gets the list of grades by the given class id
        /// </summary>
        /// <param name="classId">the class id</param>
        /// <returns>list of class grades</returns>
        Task<List<Grade>> GetGradesAsync(int classId);
        /// <summary>
        /// Get the students by grade async.
        /// </summary>
        /// <param name="gradeId">the Id of the grade</param>
        /// <returns>list of grade students</returns>
        public Task<List<AppUser>> GetStudentsAsync(int gradeId);
        /// <summary>
        /// Gets all the students by grade async.
        /// </summary>
        /// <returns>List of all the students</returns>
        public Task<List<AppUser>> GetStudentsAsync();
        /// <summary>
        /// Get the count of students async
        /// </summary>
        /// <returns>the count of the students</returns>
        public Task<int> GetStudentsCountAsync();
        Task<string> GetUserPrioritiesAsync(AppUser user);
        #endregion
        #region Creators
        /// <summary>
        /// Creates single students async.
        /// </summary>
        /// <param name="name">the name of the student.</param>
        /// <param name="gradeId">the name of the grade.</param>
        /// <returns>Whether operation succeeded</returns>
        public Task<bool> CreateAsync(string name, int gradeId);
        /// <summary>
        /// Create bulk of students async.
        /// </summary>
        /// <param name="csvStream">Stream of csv file, Windows-1255 encoded in HE or EN</param>
        /// <returns>Whether operation succeeded</returns>
        public Task<bool> CreateBulkAsync(Stream csvStream, int gradeId);
        #endregion
        #region Conference
        /// <summary>
        /// Returns whather content is selected by student
        /// </summary>
        /// <param name="student">the student</param>
        /// <param name="contentId">the pk of the content</param>
        /// <returns>whether content is selected by user</returns>
        public Task<bool> IsContentSelectedByStudentAsync(AppUser student, int contentId);
        /// <summary>
        /// returns whether student registered to conference or not;
        /// </summary>
        /// <param name="stduent">the student</param>
        /// <returns>whether the student is registered to content</returns>
        public Task<bool> IsStudentRegisteredAsync(AppUser stduent);
        /// <summary>
        /// returns all the selected contents by user
        /// </summary>
        /// <param name="student">the student</param>
        /// <returns>selected contents by user</returns>
        public Task<List<Content>> GetUserSelectedContentsAsync(AppUser student);
        /// <summary>
        /// returns a List of UserContent containing the content of the user 
        /// and including the content.
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Task<IEnumerable<IGrouping<int, ContentUser>>> GetStudentUserContetnsByTimeStripsAsync(AppUser student);
        /// <summary>
        /// returns count of registered users.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetRegisteredStudentsCountAsync();
        /// <summary>
        /// returns the list of the registered students.
        /// </summary>
        /// <returns>List of registered students</returns>
        public Task<List<AppUser>> GetRegisteredStudentsAsync();
        /// <summary>
        /// Registers the user to each content of the contents list, removing him from each content not in it.
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public Task RegisterUserToContentsAsync(AppUser student, List<Content> contents);
        #endregion
        public Task<bool> RegisterByStringAsync(AppUser student, string s);
        /// <summary>
        /// Updates the phone number and returns whether operation suceeded or not.
        /// </summary>
        /// <param name="student">the student user to update</param>
        /// <param name="num">the new number, 9-digit</param>
        /// <returns></returns>
        public Task<bool> ChangeStudentPhoneNumberAsync(AppUser student, int num);
        public Task<AppUser> FindByIdAsync(string id);
        public Task<string> GetNotRegisterdAsCsvAsync();
        public Task<List<Tuple<Content, int>>> GetContentsWithRegisteredCountAsync();
        public Task<int> HasPhoneNumberCountAsync();
        public Task<string> GenerateFullReport();
    }
    public class StudentService : IStudentService
    {
        static readonly Encoding encode = System.Text.Encoding.GetEncoding("windows-1255");
        #region Services
        private readonly AppDbContext _dbCtx;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<StudentService> _logger;
        #endregion
        #region constants
        private static string StudentRoleName = "Student";
        #endregion
        #region Ctor
        public StudentService(AppDbContext dbCtx, SignInManager<AppUser> signinManager,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            ILogger<StudentService> logger)
        {
            _dbCtx = dbCtx;
            _signinManager = signinManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        #endregion
        #region Creators
        /// <inheritdoc/>
        public async Task<bool> CreateAsync(string name, int gradeId)
        {
            // create user
            AppUser student = new AppUser() { UserName = name, GradeId = gradeId, Email = "" };
            var ir = await _userManager.CreateAsync(student);
            // add to role student
            return (await _userManager.AddToRoleAsync(student, StudentRoleName)).Succeeded;
        }
        /// <inheritdoc/>
        public async Task<bool> CreateBulkAsync(Stream csvStream, int gradeId)
        {
            try
            {
                bool isAllSuccess = true;
                using (var reader = new StreamReader(csvStream, encode))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(",");
                        _logger.LogInformation("Creating student {0} in grade PKd {1}...", values[0], gradeId);
                        // create by param and value in csv
                        await CreateAsync(values[0], gradeId);
                    }
                }
                return isAllSuccess;
            }
            catch { return false; }
        }
        #endregion
        #region Getters
        /// <inheritdoc/>
        public async Task<List<Class>> GetClassesAsync()
        {
            return await _dbCtx.Classes.ToListAsync();
        }
        /// <inhertidoc/>
        public async Task<List<Grade>> GetGradesAsync(int classId) =>
            await _dbCtx.Grades.Where(g => g.ClassId == classId).ToListAsync();
        /// <inheritdoc/>
        public async Task<List<AppUser>> GetStudentsAsync(int gradeId) =>
            await (await GetStudentsQuery()).Where(u => u.GradeId == gradeId).ToListAsync();
        public async Task<List<AppUser>> GetStudentsAsync() =>
            await (await GetStudentsQuery()).ToListAsync();
        public async Task<int> GetStudentsCountAsync() =>
            await (await GetStudentsQuery()).CountAsync();
        #endregion
        #region Conference
        /// <inhertidoc/>
        public async Task<bool> LoginAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // log in iff not admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return false;

            await _signinManager.SignInAsync(user, false);
            return true;
        }

        public async Task<bool> IsContentSelectedByStudentAsync(AppUser student, int contentId)
        {
            return (await GetUserSelectedContentsAsync(student)).Any(c => c.Id == contentId);
        }

        public async Task<bool> IsStudentRegisteredAsync(AppUser student)
        {
            var rgStudents = (await (await GetRegisteredStudentsQuery()).ToListAsync());
            return rgStudents.Any(s => s.Id == student.Id);
        }

        public async Task<List<Content>> GetUserSelectedContentsAsync(AppUser student)
        {
            var userContents = _dbCtx.ContentUsers.Where(cu => cu.UserId == student.Id);
            return await _dbCtx.Contents.Include(c => c.ContentPeople).ThenInclude(cp => cp.Person)
                .Where(c => userContents.Any(uc => uc.ContentId == c.Id)).ToListAsync();
        }

        public async Task<int> GetRegisteredStudentsCountAsync()
        {
            return await (await GetRegisteredStudentsQuery()).CountAsync();
        }
        public async Task<List<AppUser>> GetRegisteredStudentsAsync()
        {
            return await (await GetRegisteredStudentsQuery()).ToListAsync();
        }
        #endregion
        /* private base query methods */
        private async Task<IQueryable<AppUser>> GetStudentsQuery()
        {
            var roleId = (await _roleManager.FindByNameAsync(StudentRoleName)).Id;
            var userIds = _dbCtx.UserRoles.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId);
            return _dbCtx.Users.Where(u => userIds.Any(uid => uid == u.Id));
        }
        private async Task<IQueryable<AppUser>> GetRegisteredStudentsQuery()
        {
            var studentsQuery = await GetStudentsQuery();
            var regStudentIds = _dbCtx.ContentUsers.GroupBy(cu => cu.UserId).Select(gcu => gcu.Key);
            return studentsQuery.Where(s => regStudentIds.Any(rsid => rsid == s.Id));
        }

        public async Task RegisterUserToContentsAsync(AppUser student, List<Content> contents)
        {
            var allContents = await _dbCtx.Contents.ToListAsync();
            var userContents = await _dbCtx.ContentUsers.Where(cu => cu.UserId == student.Id).ToListAsync();
            foreach (var content in allContents)
            {
                var userContentInDb = userContents.FirstOrDefault(cu => cu.ContentId == content.Id);
                bool isUserWilBeRegToContent = contents.Any(c => c.Id == content.Id);
                // if user's registered
                if (userContentInDb != null)
                {
                    // but don't want to be anymore
                    if (!isUserWilBeRegToContent)
                    {
                        // remove him from db.
                        _dbCtx.ContentUsers.Remove(userContentInDb);
                    }
                }
                // if not registers
                else
                {
                    // but would like to register
                    if (isUserWilBeRegToContent)
                    {
                        // add to db.
                        await _dbCtx.ContentUsers.AddAsync(new ContentUser() { UserId = student.Id, ContentId = content.Id });
                    }
                }
            }
            await _dbCtx.SaveChangesAsync();
        }

        public async Task<bool> RegisterByStringAsync(AppUser student, string s)
        {
            string[] typesSeperated = s.Split("$");
            var contents = await _dbCtx.Contents.ToListAsync();
            var userContents = await _dbCtx.ContentUsers.Where(cu => cu.UserId == student.Id).ToListAsync();
            bool success = true;
            foreach (var typeContetns in typesSeperated)
            {
                var splittedTypeContent = typeContetns.Split('.').ToList();
                int cntr = 0;
                for (int i = 0; i <= typeContetns.Split('.').Count() - 2 && success; i++)
                {
                    int content;
                    if (int.TryParse(splittedTypeContent[i], out content))
                    {
                        if (contents.Any(c => c.Id == content))
                        {
                            cntr++;
                            var userContent = userContents.FirstOrDefault(uc => uc.ContentId == content);
                            if (userContent != null)
                            {
                                userContent.Priority = cntr;
                                userContents.Remove(userContent);
                            }
                            else
                            {
                                _dbCtx.ContentUsers.Add(new ContentUser() { ContentId = content, UserId = student.Id, Priority = cntr });
                            }
                        }
                    }
                    else
                    {
                        contents.Clear(); // stop everything!
                        success = false;
                    }
                }
                if (!success) break;
            }
            if (success)
            {
                _dbCtx.RemoveRange(userContents); // remove all left
            }
            await _dbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetUserPrioritiesAsync(AppUser user)
        {
            var contentsUser = await _dbCtx.ContentUsers.Where(cu => cu.UserId == user.Id).ToListAsync();
            if (contentsUser.Count != 0)
            {
                StringBuilder resultBuilder = new StringBuilder();
                var contents = (await _dbCtx.Contents.ToListAsync()).Where(c => contentsUser.Any(cu => cu.ContentId == c.Id)).ToList();
                bool isFirst = true;
                foreach (var timeStrip in await _dbCtx.TimeStrips.ToListAsync())
                {
                    _ = !isFirst ? resultBuilder.Append('$') : resultBuilder;
                    var curContents = contents.Where(c => c.TimeStripId == timeStrip.Id);
                    foreach (var s in curContents)
                    {
                        resultBuilder.Append(s.Id);
                        resultBuilder.Append('.');
                    }
                    isFirst = false;
                }
                return resultBuilder.ToString();
            }
            return "$$$$";
        }

        public async Task<IEnumerable<IGrouping<int, ContentUser>>> GetStudentUserContetnsByTimeStripsAsync(AppUser student)
        {
            var userContents = await _dbCtx.ContentUsers.Where(cu => cu.UserId == student.Id).Include(cu => cu.Content).ThenInclude(c => c.ContentPeople).ThenInclude(cp => cp.Person).ToListAsync();
            var timeStrips = await _dbCtx.TimeStrips.ToListAsync();
            return userContents.GroupBy(uc => uc.Content.TimeStripId).ToList();
        }

        public async Task<bool> ChangeStudentPhoneNumberAsync(AppUser student, int num)
        {
            if (num.ToString().Length != 9) return false;
            string finalNumber = "+972" + num.ToString(); // add IL prefix
            student.PhoneNumber = finalNumber;
            return (await _userManager.UpdateAsync(student)).Succeeded;
        }

        public Task<AppUser> FindByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public async Task<string> GetNotRegisterdAsCsvAsync()
        {
            var regStudents = (await GetRegisteredStudentsQuery()).Include(s => s.Grade).ThenInclude(g => g.Class).ToList();
            var allStudents = (await GetStudentsQuery()).ToList();
            var unregStudents = allStudents.Where(s => !regStudents.Any(rs => rs.Id == s.Id)).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var student in unregStudents.OrderByDescending(us => us.GradeId))
            {
                if (student.Grade != null && student.Grade.Class != null)
                    sb.AppendLine($"\"{student.UserName}\",\"{student.Grade.Class.ClassName}{student.Grade.ClassNumber}\"");
            }
            return sb.ToString();
        }

        public async Task<List<Tuple<Content, int>>> GetContentsWithRegisteredCountAsync()
        {
            var contents = await _dbCtx.Contents.ToListAsync();
            return (await _dbCtx.ContentUsers.ToListAsync()).GroupBy(cu => cu.ContentId).Select(gcu => new Tuple<Content, int>(contents.First(c => c.Id == gcu.Key), gcu.Count())).ToList();
        }

        public async Task<int> HasPhoneNumberCountAsync()
        {
            return (await _dbCtx.Users.CountAsync(u => !String.IsNullOrEmpty(u.PhoneNumber)));
        }

        public async Task<string> GenerateFullReport()
        {
            var usersContents = await _dbCtx.ContentUsers.Include(cu => cu.Content).ThenInclude(c => c.TimeStrip).Include(cu => cu.User).ThenInclude(u => u.Grade).ThenInclude(g => g.Class).ToListAsync();
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var userContent in usersContents)
            {
                resultBuilder.AppendLine($"\"{userContent.User.UserName}\"," +
                    $"\"{userContent.User.Grade.Class.ClassName.Replace("'","").Replace("\"","")}{userContent.User.Grade.ClassNumber}\"," +
                    $"\"{userContent.User.PhoneNumber}\"" +
                    $"\"{userContent.Content.Title}\"," +
                    $"\"{userContent.Content.TimeStrip.StartTime}\"," +
                    $"\"{userContent.Priority}\"," );
            }
            return resultBuilder.ToString();
        }
    }
}
