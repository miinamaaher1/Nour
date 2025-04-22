using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.Teachers;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Area("Teachers")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        async public Task<IActionResult> GetSessionAttendance([FromBody] SessionAttendanceDTO request)
        {
            var response = await _attendanceService.GetSessionAttendance(request);
            return Json(response);
        }

        [HttpPost]
        async public Task<IActionResult> SubmitSessionAttendance([FromBody] SessionAttendanceDTO request)
        {
            var response = await _attendanceService.SubmitSessionAttendance(request);
            return Json(response);
        }

    }
}
