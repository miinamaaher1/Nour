using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Services.Implementations.AssistantServices;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Area("Assistants")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IPendingRequestService _pendingRequestService;
        public HomeController(IHomeService homeService, IPendingRequestService pendingRequestService)
        {
            _homeService = homeService;
            _pendingRequestService = pendingRequestService;
        }
        // GET: HomeController
        public async Task<ActionResult> Index()
        {
            var homeVm = await _homeService.DashboardService(User);

            return View(homeVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptConfirmed(int id)
        {

            var Result = await _pendingRequestService.AcceptInvitationRequest(id);
            TempData["ToastMessage"] = "Invitation Confirmed Successfully";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            var Result = await _pendingRequestService.RejectInvitationRequest(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
