using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Area("Teachers")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ISessionService _sessionService;
        public AttendanceController(IAttendanceService attendanceService, ISessionService sessionService)
        {
            _attendanceService = attendanceService;
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        async public Task<IActionResult> GetSessionAttendance([FromBody] SessionAttendanceDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userID == null)
                return Forbid(); // or redirect to an AccessDenied view

            Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
            int teacherId = teacher.ID;
            request.TeacherId = teacherId;
            var response = await _attendanceService.GetSessionAttendance(request);
            return Json(response);
        }

        [HttpPost]
        async public Task<IActionResult> SubmitSessionAttendance([FromBody] SessionAttendanceDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userID == null)
                return Forbid(); // or redirect to an AccessDenied view

            Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
            int teacherId = teacher.ID;
            request.TeacherId = teacherId;
            var response = await _attendanceService.SubmitSessionAttendance(request);
            return Json(response);
        }

    }
}
