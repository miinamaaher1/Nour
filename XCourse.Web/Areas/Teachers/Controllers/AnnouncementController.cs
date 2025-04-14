using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Area("Teachers")]
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService announcementService)
        {
            this._announcementService = announcementService;
        }

        async public Task<IActionResult> Index(PostAnnouncementRequestDTO request)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userID != null)
            {
                Teacher teacher = await _announcementService.GetTeacherByUserId(userID);
                ViewBag.TeacherId = teacher.ID;
            } else
            {
                ViewBag.TeacherId = 0;
            }
                return View("Index");
        }

        [HttpPost]
        async public Task<IActionResult> GetAllAnnouncements([FromBody] PostAnnouncementRequestDTO request)
        {
            var announcements = await _announcementService.GetAnnouncements(request);
            return Ok(announcements);
        }

        [HttpPost]
        async public Task<IActionResult> PostAnnouncement([FromBody] PostAnnouncementRequestDTO requestDTO)
        {
            var response = await _announcementService.AddAnnouncementService(requestDTO);
            return Ok(response);
        }

        [HttpPost]
        async public Task<IActionResult> EditAnnouncement([FromBody] PostAnnouncementRequestDTO requestDTO)
        {
            var response = await _announcementService.EditAnnouncementService(requestDTO);
            return Ok(response);
        }

        [HttpPost]
        async public Task<IActionResult> DeleteAnnouncement([FromBody] PostAnnouncementRequestDTO requestDTO)
        {
            var response = await _announcementService.DeleteAnnouncementService((int)requestDTO.AnnouncementId!);
            return Ok(response);
        }

        [HttpPost]
        async public Task<IActionResult> GetAllGroups()
        {
            int teacherId = 0;
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userID != null)
            {
                Teacher teacher = await _announcementService.GetTeacherByUserId(userID);
                teacherId = teacher.ID;
            }
            var groups = await _announcementService.GetAllGroups(teacherId);  
            return Json(groups);
        }
    }
}
