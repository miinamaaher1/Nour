using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class GroupController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEnrollStudentService _enrollStudentService;
        private readonly IRequestPrivateGroupService _requestPrivateGroupService;
        IStudentGroup StudentGroup { get; set; }

        public GroupController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,
                                IEnrollStudentService enrollStudentService, 
                                IRequestPrivateGroupService requestPrivateGroupService, IStudentGroup group)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _enrollStudentService = enrollStudentService;
            _requestPrivateGroupService = requestPrivateGroupService;
            StudentGroup = group;
        }

        public async Task<IActionResult> Preview(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.AccountType != AccountType.Student)
            {
                return NotFound();
            }
            var stud = _unitOfWork.Students.Find(s => s.AppUserID == user.Id, ["Groups"]);
            var groupIds = stud.Groups.Select(g => g.ID).ToList();

            if (groupIds.Contains(id))
            {
                return RedirectToAction("Details", new { id });
            }

            var groupVM = await _enrollStudentService.GetGroupInfo(id, User);
            if (groupVM == null) return NotFound();
            return View(groupVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.AccountType != AccountType.Student)
            {
                return NotFound();
            }
            var stud = _unitOfWork.Students.Find(s => s.AppUserID == user.Id, ["Groups"]);
            var groupIds = stud.Groups.Select(g => g.ID).ToList();

            if (!groupIds.Contains(id))
            {
                return RedirectToAction("Preview", new { id });
            }

            var group = StudentGroup.Details(id);
            if (group == null) return NotFound();
            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int studentID, int groupID)
        {
            var result = await _enrollStudentService.Enroll(studentID, groupID);

            if (result)
            {
                TempData["SuccessMessage"] = "You have successfully joined the group!";
                return RedirectToAction("Details", new { id = groupID });
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to join group. Insufficient funds or the group is full.";
                return RedirectToAction("Preview", new { id = groupID });
            }
        }
        [HttpPost]
        public IActionResult PrepareRequest([FromBody] PrivateGroupParametersDTO request)
        {
            var subjectList = _requestPrivateGroupService.PrepareRequest(request.studentID, request.teacherID);
            return Json(subjectList);
        }

        [HttpPost]
        public IActionResult SendRequest([FromBody] PrivateGroupRequestDTO request)
        {
            var requestStatus = _requestPrivateGroupService.SendRequest(request);
            return Json(requestStatus);
        }

        public IActionResult MyGroups()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var groups = StudentGroup.GetStudentGroup(userID);
            if (groups == null) return NotFound();

            return View(groups);
        }



        public async Task<IActionResult> Recommended()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var groups = await StudentGroup.RecommendedGroupService(userID!);
            if (groups == null)
            {
                groups = new List<RecommendedGroupViewModel>();
            }
            return await Task.FromResult(View(groups));
        }
    }
}