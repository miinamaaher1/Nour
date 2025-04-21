using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Area("Assistants")]
    public class PendingRequestsController : Controller
    {
        private readonly IPendingRequestService _pendingRequestService;
        private readonly IUnitOfWork _unitOfWork;
        public PendingRequestsController(IPendingRequestService pendingRequestService, IUnitOfWork unitOfWork)
        {
            _pendingRequestService = pendingRequestService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var pendingRequests = await _pendingRequestService.GetPendingRequestsAsync(User);
            return View(pendingRequests);
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
