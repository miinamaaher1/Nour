using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.AssistantDTOs;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Authorize(Roles = "Assistant")]
    [Area("Assistants")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            this._attendanceService = attendanceService;
        }

        [HttpPost]
        async public Task<IActionResult> GetSessionAttendance([FromBody] SessionAttendanceDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int assistantId = 0;
            if (userID != null)
            {
                Assistant assistant = await _attendanceService.GetAssistantByUserId(userID);
                assistantId = assistant.ID;
            }
            request.AssistantId = assistantId;
            var response = await _attendanceService.GetSessionAttendance(request);
            return Json(response);
        }

        [HttpPost]
        async public Task<IActionResult> SubmitSessionAttendance([FromBody] SessionAttendanceDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int assistantId = 0;
            if (userID != null)
            {
                Assistant assistant = await _attendanceService.GetAssistantByUserId(userID);
                assistantId = assistant.ID;
            }
            request.AssistantId = assistantId;
            var response = await _attendanceService.SubmitSessionAttendance(request);
            return Json(response);
        }

    }
}
