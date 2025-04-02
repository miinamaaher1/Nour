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
        private readonly IEnrollStudentService _enrollStudentService;
        private readonly IRequestPrivateGroupService _requestPrivateGroupService;
        public GroupController(IUnitOfWork unitOfWork, IEnrollStudentService enrollStudentService, IRequestPrivateGroupService requestPrivateGroupService)
        {
            _unitOfWork = unitOfWork;
            _enrollStudentService = enrollStudentService;
            _requestPrivateGroupService = requestPrivateGroupService;
        }
        public IActionResult Details(int id)
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
                return RedirectToAction("YourGroups"); // your groups
            }
            else
            {
                return RedirectToAction("Details");
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
    }
}
