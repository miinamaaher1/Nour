using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using XCourse.Core.DTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels.Sessions;
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
        async public Task<IActionResult> GroupSessions(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                // Access Page
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }

            var sessions = await _sessionService.GetSessionsInGroup(id, teacherId);
            return View(sessions);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userID == null)
            {
                // Error Page Here
            }
            var teacher = await _sessionService.GetTeacherByUserId(userID!);
            int groupType = await _sessionService.GetGroupTypeFromSession(id, teacher.ID);
            switch (groupType)
            {
                case 0:
                    break;
                case 1:
                    return RedirectToAction(nameof(EditOnlineGroup), new { id });
                case 2:
                    return RedirectToAction(nameof(EditOfflineLocalSession), new { id });
                case 3:
                    return RedirectToAction(nameof(EditOfflineInACenter), new { id });
            }
            return null!; // error page 
        }

        public async Task<IActionResult> EditOnlineGroup(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                // Access Denied here
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }
            var session = await _sessionService.GetSessionDetailsById(id, teacherId);
            // Mapping here 
            var onlineSessionVM = new EditOnlineSessionVM();
            onlineSessionVM.StartTime = TimeOnly.FromDateTime(session.StartDateTime);
            onlineSessionVM.EndTime = TimeOnly.FromDateTime(session.EndDateTime);
            onlineSessionVM.Date = DateOnly.FromDateTime(session.StartDateTime);
            //onlineSessionVM.URL = session.URL;
            onlineSessionVM.Description = session.Description;
            onlineSessionVM.SessionID = session.ID;

            return View(onlineSessionVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditOnlineGroup(EditOnlineSessionVM sessionVM)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userID == null)
            {
                // Error Page Here
            }
            var teacher = await _sessionService.GetTeacherByUserId(userID!);
            var result = await _sessionService.EditOnlineSessionVM(sessionVM, teacher.ID);
            return RedirectToAction(nameof(Details), new {id = sessionVM.SessionID});
        }

        public async Task<IActionResult> EditOfflineLocalSession(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userID == null)
                return Forbid(); // or redirect to an AccessDenied view

            Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
            int teacherId = teacher.ID;

            var session = await _sessionService.GetSessionDetailsById(id, teacherId);

            var offlineLocalSession = new EditOfflineLocalSessionVM
            {
                SessionID = session.ID,
                StartTime = TimeOnly.FromDateTime(session.StartDateTime),
                EndTime = TimeOnly.FromDateTime(session.EndDateTime),
                Date = DateOnly.FromDateTime(session.StartDateTime),
                Description = session.Description,
                Location = session.Location,
                Address = new Core.Entities.Address
                {
                    Governorate = session.Address?.Governorate,
                    City = session.Address?.City,
                    Neighborhood = session.Address?.Neighborhood,
                    Street = session.Address?.Street
                }
            };

            return View(offlineLocalSession);
        }
        [HttpPost]
        public async Task<IActionResult> EditOfflineLocalSession(EditOfflineLocalSessionVM sessionVM)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userID == null)
            {
                // Error Page Here
            }
            var teacher = await _sessionService.GetTeacherByUserId(userID!);
            var result = await _sessionService.EditOfflineLocalSession(sessionVM, teacher.ID);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditOfflineInACenter(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                // Access Denied here
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }
            var session = await _sessionService.GetSessionDetailsById(id, teacherId);
            return View(session);
        }


         
        public async Task<IActionResult> AddSession(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                // Access Page
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }

            int groupType = await _sessionService.GetGroupTypeById(id, teacherId);
            switch (groupType)
            {
                case 1:
                    return RedirectToAction(nameof(AddOnlineSession), new { id = id });
                default:
                    return null;
            }
        }
        
        // Adding Online session
        public async Task<IActionResult> AddOnlineSession(int id)
        {
            AddOnlineSessionVM sessionVM = new AddOnlineSessionVM();
            sessionVM.GroupID = id;
            return View(sessionVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddOnlineSession(AddOnlineSessionVM sessionVM)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }
            if (!ModelState.IsValid)
            {
                return View(sessionVM);
            }
            var response = await _sessionService.AddOnlineSession(sessionVM, teacherId);
            if (response.Status == true)
            {
                return RedirectToAction(nameof(GroupSessions), new {id = sessionVM.GroupID});
            }
            else
            {
                foreach (var err in response.Errors!)
                {
                    ModelState.AddModelError(string.Empty, err);
                };
                return View(sessionVM);
            }
        }


        // Adding Local offline local session
        public async Task<IActionResult> AddOfflineLocalSession(int id)
        {
            AddOfflineLocalSessionVM sessionVM = new AddOfflineLocalSessionVM();
            sessionVM.GroupID = id;
            sessionVM.Location = new NetTopologySuite.Geometries.Point(10, 20) { SRID = 4326 };
            var now = DateTime.Now;
            sessionVM.StartTime = new TimeOnly(now.Hour, now.Minute);
            sessionVM.EndTime = sessionVM.StartTime.AddHours(1);
            sessionVM.Date = DateOnly.FromDateTime(now.AddDays(1));
            return View(sessionVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddOfflineLocalSession(AddOfflineLocalSessionVM sessionVM)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int teacherId = 0;
            if (userID != null)
            {
                Teacher teacher = await _sessionService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }
            if (sessionVM.Latitude.HasValue && sessionVM.Longitude.HasValue)
            {
                sessionVM.Location = new NetTopologySuite.Geometries.Point(sessionVM.Latitude.Value, sessionVM.Longitude.Value) { SRID = 4326 };
            }
            else
            {
                sessionVM.Location = null;
            }
            if (!ModelState.IsValid)
            {
                return View(sessionVM);
            }
            var response = await _sessionService.AddOfflineLocalSession(sessionVM, teacherId);
            if (response.Status == true)
            {
                return RedirectToAction(nameof(GroupSessions), new { id = sessionVM.GroupID });
            }
            else
            {
                foreach (var err in response.Errors!)
                {
                    ModelState.AddModelError(string.Empty, err);
                }
                ;
                return View(sessionVM);
            }

        }



    }
}
