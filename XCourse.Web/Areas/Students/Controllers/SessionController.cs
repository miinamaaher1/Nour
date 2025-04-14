using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Services.Interfaces.Student;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class SessionController : Controller
    {
        private IStudentHomeService _studentHomeService;

        public SessionController(IStudentHomeService studentHomeService )
        {
            _studentHomeService = studentHomeService;

        }
        public async Task<IActionResult> Index(int? id) // id is the group id (nullable)
        {

            var sessionsVM = await _studentHomeService.SessionIndexService(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!, id ?? 0);

            return View(sessionsVM);
        }
        public async Task<IActionResult> Details(int Id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            var sessionDetailsVM = await _studentHomeService.SessionDetailsService(Id,userId);
            return View(sessionDetailsVM);
        }


        [HttpPost]
        public async Task<IActionResult> SaveFeedback([FromBody] FeedBackDTO feedBackDTO)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            bool isSaved = await _studentHomeService.SessionSaveFeedbackService(feedBackDTO, userId);

            return Json(new { isValid = isSaved });
        }
    }
}
