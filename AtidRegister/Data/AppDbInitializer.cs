using AtidRegister.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AtidRegister.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtidRegister.Data
{
    /// <summary>
    /// Database initializing by appsettings and app requirments
    /// </summary>
    public class AppDbInitializer
    {
        /// <summary>
        /// Initializes the Database
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            var options = serviceProvider.GetService<IOptions<AppConfig>>().Value;
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            context.Database.Migrate(); // make sure db is migrated
            // make sure there are roles
            if (!context.Roles.Any(r => r.Name == "Admin"))
                Task.WaitAll(roleManager.CreateAsync(new IdentityRole("Admin")));
            if (!context.Roles.Any(r => r.Name == "Student"))
                Task.WaitAll(roleManager.CreateAsync(new IdentityRole("Student")));
            var adminRoleId = context.Roles.FirstOrDefault(r => r.Name == "Admin").Id;
            // make sure there's admin'Cannot resolve scoped service 'Microsoft.AspNetCore.Identity.RoleManager`1[Microsoft.AspNetCore.Identity.IdentityRole]' from root provider.'
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = new AppUser()
            {
                Email = options.DefaultAdminUser.Email,
                UserName = options.DefaultAdminUser.UserName
            };
            var fu = context.Users.FirstOrDefault(u => u.UserName == options.DefaultAdminUser.UserName);
            if (fu == null)
            {
                Task.WaitAll(userManager.CreateAsync(user, options.DefaultAdminUser.Password));
                Task.WaitAll(userManager.AddToRoleAsync(user, "Admin"));
            }
            else if (!context.UserRoles.Any(ur => ur.UserId == fu.Id && adminRoleId == ur.RoleId))
                Task.WaitAll(userManager.AddToRoleAsync(user, "Admin"));
        }
    }
}
