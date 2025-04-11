using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class GroupController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnrollStudentService _enrollStudentService;
        private readonly IRequestPrivateGroupService _requestPrivateGroupService;
        IStudentGroup StudentGroup { get; set; }

        public GroupController(IUnitOfWork unitOfWork, IEnrollStudentService enrollStudentService, IRequestPrivateGroupService requestPrivateGroupService, IStudentGroup group)
        {
            _unitOfWork = unitOfWork;
            _enrollStudentService = enrollStudentService;
            _requestPrivateGroupService = requestPrivateGroupService;
            StudentGroup = group;
        }



        public IActionResult DetailsNotEnrolled(int id)
        {
            var group = _unitOfWork.Groups.Find(g => g.ID == id, ["Subject", "Teacher.AppUser", "GroupDefaults"]);
            if (group == null) return NotFound();
            return View(group);
        }

        [HttpPost]
        public IActionResult Enroll(int studentID, int groupID)
        {
            if (_enrollStudentService.Enroll(studentID, groupID))
            {
                return RedirectToAction("Details", new { id = groupID }); // your groups
            }
            else
            {
                return RedirectToAction("DetailsNotEnrolled", new { id = groupID });
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

        public IActionResult getAll()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var groups = StudentGroup.GetStudentGroup(userID);
            if (groups == null) return NotFound();

            return View(groups);
        }

        public IActionResult Details(int id)
        {
            var group = StudentGroup.Details(id);
            if (group == null) return NotFound();

            return View(group);
        }

        public async Task<IActionResult> RecommendedGroups()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var groups = await StudentGroup.RecommendedGroupService(userID!);
            if (groups == null)
            {
                groups = new List<RecommendedGroupViewModel>();
            }
            return await Task.FromResult( View(groups));
        }
    }
}