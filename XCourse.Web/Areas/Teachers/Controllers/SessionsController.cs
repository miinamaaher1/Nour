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
            this._sessionService = sessionService;
        }
        public IActionResult Index()
        {
            return View();
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
