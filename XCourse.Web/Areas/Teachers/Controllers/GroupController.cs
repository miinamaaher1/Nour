using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Text.Json;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Services.Interfaces.Teachers;

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

        // Group Creation 
        public IActionResult CreateOffline()
        {
            return View();
        }
        public IActionResult CreateOfflineLocal()
        {
            return View();
        }
        public IActionResult CreateOnline()
        {
            return View();
        }

        // Group Details 
        public async Task<IActionResult> Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Teacher teacher = await _groupService.GetTeacherByUserId(userID!);
            IEnumerable<GroupVM> groupVMs = await _groupService.GetAllGroups(teacher.ID);
            return View(groupVMs);
        }
        public async Task<IActionResult> Details(int id)
        {
            var groupDetailsVM = await _groupService.GetGroupDetailsById(id);
            return View(groupDetailsVM);
        }



        // Reservation endpoints
        [HttpPost]
        public async Task<IActionResult> ReserveGroupInCenter([FromBody] RequestOfflineGroupReservation request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Teacher teacher = await _groupService.GetTeacherByUserId(userID!);
            request.TeacherId = teacher.ID;
            var result = await _groupService.ReserveAnOfflineGroupInCenter(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ReserveOnlineGroup([FromBody] ReserveOnlineGroupRequestDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Teacher teacher = await _groupService.GetTeacherByUserId(userID!);
            request.TeacherId = teacher.ID;
            var result = await _groupService.ReserveOnlineGroup(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ReserveOfflineLocalGroup([FromBody] ReserveOfflineLocalGroupRequestDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Teacher teacher = await _groupService.GetTeacherByUserId(userID!);
            request.TeacherId = teacher.ID;
            var result = await _groupService.ReserveOfflineLocalGroup(request);
            return Ok(result);
        }


        // Utils
        [HttpPost]
        public async Task<IActionResult> Years()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userID))
            {
                return Json(new List<int>());
            }

            var teacher = await _groupService.GetTeacherByUserId(userID);

            if (teacher == null || teacher.ID == 0)
            {
                return Json(new List<int>());
            }

            var years = await _groupService.GetAllMatchingYears(teacher.ID);

            return Json(years);
        }

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
        public async Task<IActionResult> Subjects([FromBody] RequestSubjectDto request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Teacher teacher = await _groupService.GetTeacherByUserId(userID!);
            request.TeacherId = teacher.ID;
            var subjects = await _groupService.GetMatchingSubjects(request);
            return Ok(subjects);
        }
        [HttpPost]
        public async Task<IActionResult> AllSubjects([FromBody] RequestSubjectDto request)
        {
            var subjects = await _groupService.GetAllSubjects(request);
            return Ok(subjects);
        }
        [HttpPost]
        public async Task<bool> InsertAnnouncement([FromBody]RequestAnnouncement request)
        {
            //var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            //Teacher teacher = await _groupService.GetTeacherByUserId(userID!);
            if(request.title == "")
            {
                request.title = null;
            }
            return await _groupService.PostAnnouncement(request.groupId, 1, request.body! ,request.isImportant, request.title);

        }
        [HttpPost]
        public async Task<IActionResult> GetAllGroups()
        {
            IEnumerable <GroupVM> groupDetails = await _groupService.GetAllGroups(1);
            return Json(groupDetails);
        } 
        
    }
}
