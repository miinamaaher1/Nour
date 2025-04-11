using Microsoft.AspNetCore.Mvc;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Area("Teachers")]
    public class SessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
