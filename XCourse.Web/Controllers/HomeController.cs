using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels;


namespace XCourse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.AccountType == AccountType.Student)
            {
                return RedirectToAction("Index", "Home", new { area = "Students" });
            }
            else if (user?.AccountType == AccountType.Teacher)
            {
                return RedirectToAction("Index", "Home", new { area = "Teachers" });
            }
            else if (user?.AccountType == AccountType.CenterAdmin)
            {
                return RedirectToAction("Index", "Center", new { area = "CenterAdmins" });
            }
            //else if (user?.AccountType == AccountType.Assistant)
            //{
            //    return RedirectToAction("Index", "Home", new { area = "Assistants" });
            //}
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
