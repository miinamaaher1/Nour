using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Area("Teachers")]
    public class AssistantController : Controller
    {
        private readonly IAssistantService _assistantService;
        public AssistantController(IAssistantService assistantService)
        {
            this._assistantService = assistantService;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id <= 0)
                return Forbid();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid();

            var teacher = await _assistantService.GetTeacherByUserId(userId);
            if (teacher == null)
                return Forbid();

            var group = await _assistantService.GetGroupById(id, teacher.ID);
            if (group == null)
                return Forbid();

            var assistants = await _assistantService.GetAssistantsInGroup(teacher.ID, id);

            ViewBag.Group = group;
            return View(assistants);
        }
        
        public async Task<IActionResult> Assign(int id)
        {
            if (id <= 0)
                return Forbid();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid();

            // Get the teacher by user ID
            var teacher = await _assistantService.GetTeacherByUserId(userId);
            if (teacher == null)
                return Forbid();

            // Get the group for the teacher
            var group = await _assistantService.GetGroupById(id, teacher.ID);
            if (group == null)
                return Forbid();

            // Only fetch assistants *after* group is validated
            var assistants = await _assistantService.GetRecommendedAssistants(group.ID);

            ViewBag.Group = group;
            ViewBag.Assistants = assistants;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(InviteAssistantDTO inviteAssistantDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid();

            var teacher = await _assistantService.GetTeacherByUserId(userId);
            if (teacher == null)
                return Forbid();

            var group = await _assistantService.GetGroupById(inviteAssistantDTO.GroupId, teacher.ID);
            if (group == null)
                return Forbid();

            bool success = await _assistantService.AssignAssistantToGroup(
                inviteAssistantDTO.AssistantId,
                inviteAssistantDTO.GroupId,
                teacher.ID
            );

            if (!success)
                return Forbid();

            return RedirectToAction(nameof(Index), new { id = inviteAssistantDTO.GroupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccess(int groupId, int assistantId)
        {
            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid();

            var teacher = await _assistantService.GetTeacherByUserId(userId);
            if (teacher == null)
                return Forbid();

            var group = await _assistantService.GetGroupById(groupId, teacher.ID);
            if (group == null)
                return Forbid();

            bool res = await _assistantService.RemoveAssistantAccess(assistantId, groupId, teacher.ID);
            if (res)
            {
                return RedirectToAction(nameof(Index), new { id = groupId });
            }

            return Forbid(); 
        }

        public async Task<IActionResult> AssignedGroups(int id)
        {
            if (id <= 0)
                return Forbid();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Forbid();

            var teacher = await _assistantService.GetTeacherByUserId(userId);
            if (teacher == null)
                return Forbid();

            // Get assistant
            var assistant = await _assistantService.GetAssistantById(id);
            if (assistant == null)
                return NotFound();
            var assistantName = (assistant.AppUser?.FirstName ?? "") + " " + (assistant.AppUser?.LastName ?? "");
            ViewBag.Assistant = assistantName.Trim();

            // Get groups assigned to this assistant (for this teacher)
            var groups = await _assistantService.GetAccessedGroups(teacher.ID, id);
            return View(groups);
        }
    }
}
