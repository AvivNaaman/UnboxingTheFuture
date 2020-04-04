using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtidRegister.Configuration;
using AtidRegister.Data;
using AtidRegister.Models;
using AtidRegister.Services;
using AtidRegister.Services.Conference;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AtidRegister
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Hebrew Encoding Povider Registration
            EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            // add MVC & runtime compialtion for develpoment
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            // Add db
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlite(Configuration.GetConnectionString("App"));
            });
            // Add identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // password can be at least 4 digit. no other reqs.
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                // user names can be real names in hebrew.
                options.User.AllowedUserNameCharacters = String.Empty;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            // General user service
            services.AddTransient<IUserService, UserService>();
            // Conference (Content, People and Types) services
            services.AddTransient<IContentsService, ContentsService>();
            services.AddTransient<IPeopleService, PeopleService>();
            // Student service (registration, create, delete, get, data, stats)
            services.AddTransient<IStudentService, StudentService>();
            // Admin service 
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IFAQsService, FAQsService>();
            // configur login path
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login"; // login path
                options.AccessDeniedPath = "/Home/AccessDenied"; // access denied path
                options.LogoutPath = "/Home/Logout"; // logout path
                // auto logout after X minutes of inactivity.
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(Configuration.GetSection("AppConfig").Get<AppConfig>().AdminMinutesTimeout);
            });
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // dev excetion page
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // redirect to https
            app.UseHttpsRedirection();
            // use static files
            app.UseStaticFiles();

            app.UseRouting();

            // auths
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // admin area
                endpoints.MapAreaControllerRoute(
                    "admin",
                    "admin",
                    "Admin/{controller=Home}/{action=Index}/{id?}");
                // normal MVC
                endpoints.MapControllerRoute(
                    "default", "{controller=Home}/{action=Index}/{id?}");
            });
            // initialize db
            AppDbInitializer.Initialize(app.ApplicationServices);
        }
    }
}
