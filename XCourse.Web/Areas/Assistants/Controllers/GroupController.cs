using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Area("Assistants")]
    public class GroupController : Controller
    {
        // GET: GroupController
        private readonly IGroupService _groupService;
        private readonly IGroupDetailsService _groupDetailsService;
        public GroupController(IGroupDetailsService groupDetailsService ,IGroupService groupService)
        {
            _groupService = groupService;
            _groupDetailsService = groupDetailsService;
        }
        public async Task<IActionResult> Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var groups = await _groupService.GetAllAcceptedGroupsAsync(User);
            return View(groups);
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
