using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Services.Implementations.StudentServices;
using XCourse.Services.Interfaces.StudentServices;


namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class GroupController : Controller
    {
        IStudentGroup StudentGroup { get; set; }
        public GroupController(IStudentGroup group) {
        
        StudentGroup = group;
        }
        // GET: GroupController
       
        public IActionResult getAll()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var groups =  StudentGroup.GetStudentGroup(userID);
            if (groups == null) return NotFound();

            return View(groups);
        }

        public IActionResult Details(int id)
        {
            var group = StudentGroup.Details(id);
            if (group == null) return NotFound();
          
            return View(group);
        }



    }
}
