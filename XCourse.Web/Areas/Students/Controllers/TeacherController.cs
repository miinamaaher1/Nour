using Microsoft.AspNetCore.Mvc;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class TeacherController : Controller
    {
        private readonly ITeacherProfileService _teacherProfileService;
        public TeacherController(ITeacherProfileService teacherProfileService)
        {
            _teacherProfileService = teacherProfileService;
        }
        public IActionResult Profile(int id)
        {
            TeacherProfileVM model = _teacherProfileService.CompileTeacherProfile(id, User);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
