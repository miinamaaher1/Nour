using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.Teachers;
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

        [HttpPost]
        async public Task<IActionResult> Index()
        {
            var announcements = await _announcementService.GetAnnouncements(1,0,0);
            return Json(announcements);
        }

        [HttpPost]
        async public Task<IActionResult> PostAnnouncement([FromBody]PostAnnouncementRequestDTO requestDTO)
        {
            PostAnnouncementResponseDTO responseDTO = new PostAnnouncementResponseDTO
            {
                IsValid = true,
                Errors = new List<string>()
            };

            bool inValidRequest = false;

            if (requestDTO.AnnouncementTitle == "")
            {
                responseDTO.IsValid = false;
                responseDTO.Errors.Add("No title provided!");
                inValidRequest = true;
            }
            if (requestDTO.AnnouncementBody == "")
            {
                responseDTO.IsValid = false;
                responseDTO.Errors.Add("No content provided!");
                inValidRequest = true;
            }
            if (requestDTO.GroupsIds == null || requestDTO.GroupsIds.Length == 0)
            {
                responseDTO.IsValid = false;
                responseDTO.Errors.Add("No target groups provided!");
                inValidRequest = true;
            }
            if (inValidRequest)
            {
                return BadRequest(responseDTO);
            }

            responseDTO = await  _announcementService.PostAnnouncement(1, requestDTO.GroupsIds!, requestDTO.AnnouncementBody, requestDTO.AnnouncementTitle);
            if (responseDTO.IsValid)
            {
                return Ok(responseDTO);
            } else
            {
                return BadRequest(responseDTO);
            }
        }
    }
}
