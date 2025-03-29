using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using XCourse.Core.DTOs.Teacher;
using XCourse.Services.Interfaces.Teacher;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Area("Teachers")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            this._groupService = groupService;
        }

        public IActionResult Create()
        {
            return View();
        }




        // JSON Actions
        [HttpPost]
        public async Task<IActionResult> Centers([FromBody] RequestCenterDto request)
        {
            var centers = await _groupService.GetAllCentersPerGovernorate(request.Governorate);
            return Ok(centers);
        }
        [HttpPost]
        public async Task<IActionResult> Rooms([FromBody] RequestRoomDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request payload");
            }

            var rooms = await _groupService.GetAllAvailableRooms(request);
            return Ok(rooms);
        }
        [HttpPost]
        public async Task<IActionResult> Subjects ([FromBody] RequestSubjectDto request)
        {
            var subjects = await _groupService.GetMatchingSubjects(request);
            return Ok(subjects);
        }
    }
}
