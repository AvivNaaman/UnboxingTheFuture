using AtidRegister.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Services
{
    public interface IAdminService
    {
        /// <summary>
        /// Log admin in async
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns>Whether operation succeeded</returns>
        public Task<bool> LoginAsync(string userName, string password);
        /// <summary>
        /// Returns Whether current user is admin
        /// </summary>
        /// <returns></returns>
        Task<bool> IsCurrentUserAdminAsync();
    }
    public class AdminService : IAdminService
    {
        #region Services
        private readonly SignInManager<AppUser> _singInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion
        #region Ctor
        public AdminService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _singInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion
        /// <inheritdoc/>
        public async Task<bool> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            // make sure user's admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return (await _singInManager.PasswordSignInAsync(user, password, false, false)).Succeeded;
            return false;
        }

        public async Task<bool> IsCurrentUserAdminAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                return await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Admin");
            }
            return false;
        }
    }
}
