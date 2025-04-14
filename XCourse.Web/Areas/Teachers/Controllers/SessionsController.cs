using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Area("Teachers")]
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionsController( ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(int? groupId=null)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var sessions =await  _sessionService.GetSessionsPerTeacher(userID!, groupId);
            return View(sessions);
        }
    }
}
