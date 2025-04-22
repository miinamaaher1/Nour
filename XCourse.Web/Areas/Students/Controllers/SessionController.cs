using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs;
using XCourse.Services.Interfaces.Student;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Authorize(Roles = "Student")]
    [Area("Students")]
    public class SessionController : Controller
    {
        private IStudentHomeService _studentHomeService;

        public SessionController(IStudentHomeService studentHomeService)
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
            var sessionDetailsVM = await _studentHomeService.SessionDetailsService(Id, userId);
            return View(sessionDetailsVM);
        }

        [HttpPost]
        public async Task<IActionResult> PayForSession([FromBody] PayRequestDTO paymentRequest)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            bool isSuccess = await _studentHomeService.PayForSessionServiceAsync(paymentRequest.SessionId, userId);

            if (isSuccess)
            {
                return Json(new { isValid = true, message = "Payment successful" });
            }
            else
            {
                return Json(new { isValid = false, message = "Payment failed. Please check your wallet balance." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveFeedback([FromBody] FeedBackDTO feedBackDTO)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            bool isSaved = await _studentHomeService.SessionSaveFeedbackService(feedBackDTO, userId);

            return Json(new { isValid = isSaved });
        }

        public async Task<IActionResult> RemoveFeedback([FromBody] FeedBackDTO feedBackDTO)
        {
            bool isRemoved = await _studentHomeService.SessionRemoveFeedbackService(feedBackDTO);
            return Json(new { isValid = isRemoved });
        }
    }
}
