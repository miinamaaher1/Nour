using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.TeacherServices;
using XCourse.Web.Areas.Students.Controllers;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Area("Teachers")]
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(int? groupId = null)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var sessions = await _sessionService.GetSessionsPerTeacher(userID!, groupId);
            return View(sessions);
        }
        async public Task<IActionResult> Details(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }
            var session = await _sessionService.GetSessionDetailsById(id, teacherId);

            return View(session);
        }
    }
}
