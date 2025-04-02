using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Xcourse.Infrastructure.Repositories;
using XCourse.Core.DTOs;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Implementations.EmailServices;
using XCourse.Web.ServicesCollections;

namespace XCourse.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //var connectionString = builder.Configuration.GetConnectionString("TestConnection");

            builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));

            builder.Services.AddDbContext<XCourseContext>(options => options.UseSqlServer(connectionString, options => options.UseNetTopologySuite()));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, GmailSender>();
            builder.Services.AddStudentServices();
            builder.Services.AddTeacherServices();

            // Add CORS policy
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowLocalhost3000",
            //        policy =>
            //        {
            //            policy.WithOrigins("http://127.0.0.1:3000")
            //                .AllowAnyMethod()
            //                .AllowAnyHeader();
            //        });
            //});

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;

            }).AddEntityFrameworkStores<XCourseContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                var googleAuthNSection = builder.Configuration.GetSection("GmailSettings");
                options.ClientId = googleAuthNSection["ClientId"]!;
                options.ClientSecret = googleAuthNSection["ClientSecret"]!;
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Enable CORS before adding controllers
            //app.UseCors("AllowLocalhost3000");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            ).WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            ).WithStaticAssets();

            app.Run();
        }
    }
}
