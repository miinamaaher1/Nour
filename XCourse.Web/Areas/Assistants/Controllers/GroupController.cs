using Microsoft.AspNetCore.Mvc;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Area("Assistants")]
    public class GroupController : Controller
    {
        private readonly IGroupDetailsService _groupDetailsService;
        public GroupController(IGroupDetailsService groupDetailsService)
        {
            _groupDetailsService = groupDetailsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
             
            var groupDetailsVM = _groupDetailsService.Details(id);
            if (groupDetailsVM == null)
            {
                return NotFound();
            }
            return View(groupDetailsVM);

        }
    }
}
