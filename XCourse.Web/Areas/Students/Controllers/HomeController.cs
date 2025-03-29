using Microsoft.AspNetCore.Mvc;
using XCourse.Core.ViewModels.Students;
using XCourse.Services.Interfaces.Student;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class HomeController : Controller
    {
        private readonly IStudentHomeService _studentHomeService;
        public HomeController(IStudentHomeService studentHomeService)
        {
            _studentHomeService = studentHomeService;

        }
        public IActionResult Index()
        {
            //ahmed ===> 9798ruhuhvvrnr84994
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            HomeViewModel homeViewModel= _studentHomeService.IndexService(userID!);

            return View(homeViewModel);
        }
    }
}
