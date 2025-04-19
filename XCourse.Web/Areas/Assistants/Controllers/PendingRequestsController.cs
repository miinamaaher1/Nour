using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Implementations.AssistantServices;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Area("Assistants")]
    public class PendingRequestsController : Controller
    {
        private readonly IPendingRequestService _pendingRequestService;
        private readonly IUnitOfWork _unitOfWork;
        public PendingRequestsController(IPendingRequestService pendingRequestService,IUnitOfWork unitOfWork)
        {
            _pendingRequestService = pendingRequestService;
            _unitOfWork = unitOfWork;
        }

        // GET: PendingRequestsController
        public async Task<IActionResult> Index()
        {
            var pendingRequests = await _pendingRequestService.GetPendingRequestsAsync(User);
            return View(pendingRequests);
        }

        [HttpGet]
        public async Task<IActionResult> Accept(int id)
        {
            var Result = await _pendingRequestService.AcceptInvitationRequest(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Reject(int id)
        {
            var pendingRequest = await _pendingRequestService.FindInvitationRequestByID(id);
            return View(pendingRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            var Result = await _pendingRequestService.RejectInvitationRequest(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
