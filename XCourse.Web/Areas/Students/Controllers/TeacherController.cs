using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Authorize(Roles = "Student")]
    [Area("Students")]
    public class TeacherController : Controller
    {
        private readonly ITeacherProfileService _teacherProfileService;
        public TeacherController(ITeacherProfileService teacherProfileService)
        {
            _teacherProfileService = teacherProfileService;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherProfileService.GetAllTeachersAsync(User);
            return View(teachers);
        }
        public IActionResult Profile(int id)
        {
            TeacherProfileVM model = _teacherProfileService.CompileTeacherProfile(id, User);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
