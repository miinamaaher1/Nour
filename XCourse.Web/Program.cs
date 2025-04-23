using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xcourse.Infrastructure.Repositories;
using XCourse.Core.DTOs;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Web.ServicesCollections;
using XCourse.Web.Middleware;

namespace XCourse.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));

            builder.Services.AddDbContext<XCourseContext>(options => options.UseSqlServer(connectionString, options => options.UseNetTopologySuite()));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddStudentServices();
            builder.Services.AddTeacherServices();
            builder.Services.AddIntegratedServices();
            builder.Services.AddCenterAdminServices();
            builder.Services.AddAssistantServices();
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", policy =>
            //    {
            //        policy.AllowAnyOrigin()
            //              .AllowAnyMethod()
            //              .AllowAnyHeader();
            //    });
            //});


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                //options.Password.RequireDigit = false;
                //options.Password.RequireLowercase = false;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequiredLength = 4;

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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseStaticFiles();

            app.UseRouting();
            Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            //app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Subject}/{action=Index}"
            ).WithStaticAssets();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            ).WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            ).WithStaticAssets();

            // seed app with roles

            using (var scope = app.Services.CreateScope())
            {
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Student", "Teacher", "CenterAdmin", "Assistant" };

                foreach (var role in roles)
                {
                    if (!await roleMgr.RoleExistsAsync(role))
                    {
                        await roleMgr.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            app.Run();
        }
    }
}
