using AtidRegister.Configuration;
using AtidRegister.Data;
using AtidRegister.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtidRegister.Services
{
    public interface IUserService
    {
        /// <summary>
        /// bulky creates users by specified csv file.
        /// </summary>
        /// <param name="csvPath"></param>
        /// <returns></returns>
        public Task<bool> BulkCreateAsync(Stream csvFileStream);
        /// <summary>
        /// authenticates user using id.
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public Task<bool> AuthenticateAsync(int idNumber);
        public Task<List<AppUser>> GetNonAdminUsersAsync();

        /// <summary>
        /// authenticates user using id & password.
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<bool> AuthenticateAsync(int idNumber, string password);
        /// <summary>
        /// removes user.
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public Task RemoveAsync(int idNumber);
        /// <summary>
        /// creates admin user
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task CreateAdminAsync(string userName, string password, string email);
        public Task<List<AppUser>> GetAdminsAsync();

        /// <summary>
        /// returns whether specfied user is admin.
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public Task<bool> IsAdminAsync(int idNumber);
        /// <summary>
        /// logs out the user
        /// </summary>
        /// <returns></returns>
        public Task Logout();
        /// <summary>
        /// gets the currently logged-in user.
        /// </summary>
        /// <returns></returns>
        public Task<AppUser> GetCurrentUserAsync();
        /// <summary>
        /// returns the non-admin users count.
        /// </summary>
        /// <returns></returns>
        public Task<List<AppUser>> GetNonRegisteredAdminsAsync();
        public Task<List<AppUser>> GetNonUnregisteredAdminsCountAsync();
        public Task<List<AppUser>> GetUsersAsync();
        public Task CreateSingleStudentsAsync(int idNumber, string name, string grade);
        public Task<int> GetNonAdminsCountAsync();
    }
    public class UserService : IUserService
    {
        public static string AdminRoleName = "Admin";

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInMgr;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _ctx;
        private readonly ILogger<UserService> _logger;

        static readonly Encoding encode = System.Text.Encoding.GetEncoding("windows-1255");

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInMgr,
                            RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor,
                            AppDbContext ctx, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInMgr = signInMgr;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _ctx = ctx;
            _logger = logger;
        }
        public async Task<bool> AuthenticateAsync(int idNumber)
        {
            var user = await _userManager.FindByNameAsync(idNumber.ToString());
            if (user != null)
            {
                await _signInMgr.SignInAsync(user, false);
                return true;
            }
            return false;
        }
        public async Task<bool> AuthenticateAsync(int idNumber, string password)
        {
            var user = await _userManager.FindByNameAsync(idNumber.ToString());
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    return await AuthenticateAsync(idNumber);
                }
            }
            return false;
        }
        /// <summary>
        /// Creates bulk user by csv file.
        /// IdNumber,FriendlyName,Grade
        /// </summary>
        /// <param name="csvPath">the csv file path</param>
        /// <returns>whether operation succeeded or not.</returns>
        public async Task<bool> BulkCreateAsync(Stream csvFileStream)
        {
            try
            {
                bool isAllSuccess = true;
                using (var reader = new StreamReader(csvFileStream, encode))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(",");
                        var res = await _userManager.CreateAsync(new AppUser() { Email = "", UserName = values[0], FriendlyName = values[1] });
                        _logger.LogInformation("Creating user {0} | {1} | {2}...", values[0], values[1], values[2]);
                        if (res.Succeeded)
                        {
                            _logger.LogInformation("Success");
                        }
                        else
                        {
                            isAllSuccess = false;
                            _logger.LogError("FAILED!, error: {0}", res.Errors.Select(ie => ie.Description));
                        }
                    }
                }
                return isAllSuccess;
            }
            catch { return false; }
        }

        public async Task CreateAdminAsync(string username, string password, string email)
        {
            // create user with password
            var user = new AppUser() { Email = email, UserName = username };
            await _userManager.CreateAsync(user, password);
            // add to role
            await _userManager.AddToRoleAsync(user, AdminRoleName);
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                return await _userManager.GetUserAsync(user);
            }
            return null;
        }



        public async Task<List<AppUser>> GetUsersAsync() => await _ctx.Users.ToListAsync();

        public async Task<bool> IsAdminAsync(int idNumber)
        {
            var user = await _userManager.FindByNameAsync(idNumber.ToString());
            if (user != null)
                if (await _userManager.IsInRoleAsync(user, AdminRoleName))
                    return true;
            return false;
        }

        public async Task Logout()
        {
            await _signInMgr.SignOutAsync();
        }

        public async Task RemoveAsync(int idNumber)
        {
            var user = await _userManager.FindByNameAsync(idNumber.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<List<AppUser>> GetAdminsAsync()
        {
            var roleId = (await _ctx.Roles.FirstOrDefaultAsync(r => r.Name == AdminRoleName)).Id;
            return await _ctx.UserRoles.Where(ur => ur.RoleId == roleId)
                .Select(ur => _userManager.Users.FirstOrDefault(u => u.Id == ur.UserId))
                .ToListAsync();
        }

        public Task CreateSingleStudentsAsync(int idNumber, string name, string grade)
        {
            return null;
        }

        public async Task<List<AppUser>> GetNonAdminUsersAsync()
        {
            var roleId = (await _ctx.Roles.FirstOrDefaultAsync(r => r.Name == AdminRoleName)).Id;
            var adminIds = await _ctx.UserRoles.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId).ToListAsync();
            return await _ctx.Users.Where(u => !adminIds.Any(aid => aid == u.Id)).ToListAsync();
        }

        public async Task<int> GetNonAdminsCountAsync()
        {
            return (await GetNonAdminUsersAsync()).Count;
        }

        public async Task<List<AppUser>> GetNonRegisteredAdminsAsync()
        {
            var roleId = (await _ctx.Roles.FirstOrDefaultAsync(r => r.Name == AdminRoleName)).Id;
            var adminIds = await _ctx.UserRoles.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId).ToListAsync();
            var regIds = await _ctx.ContentUsers.GroupBy(cu => cu.UserId).Select(gcu => gcu.Key).ToListAsync();
            return await _ctx.Users.Where(u => !adminIds.Any(aid => aid == u.Id) && regIds.Any(rid => rid == u.Id)).ToListAsync();
        }
        public async Task<List<AppUser>> GetNonUnregisteredAdminsCountAsync()
        {

            var roleId = (await _ctx.Roles.FirstOrDefaultAsync(r => r.Name == AdminRoleName)).Id;
            var adminIds = await _ctx.UserRoles.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId).ToListAsync();
            var regIds = await _ctx.ContentUsers.GroupBy(cu => cu.UserId).Select(gcu => gcu.Key).ToListAsync();
            return await _ctx.Users.Where(u => !adminIds.Any(aid => aid == u.Id) && !regIds.Any(rid => rid == u.Id)).ToListAsync();
        }
    }
}
