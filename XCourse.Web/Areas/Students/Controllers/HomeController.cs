using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.ViewModels.StudentsViewModels;
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
        public async Task<IActionResult> Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            HomeViewModel homeViewModel= await  _studentHomeService.IndexService(userID!);

            return View(homeViewModel);
        }
    }
}
