using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Xcourse.Infrastructure.Repositories;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Implementations;

namespace XCourse.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //var connectionString = builder.Configuration.GetConnectionString("TestConnection");

            builder.Services.AddDbContext<XCourseContext>(options => options.UseSqlServer(connectionString, options => options.UseNetTopologySuite()));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;

            }).AddEntityFrameworkStores<XCourseContext>().AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
