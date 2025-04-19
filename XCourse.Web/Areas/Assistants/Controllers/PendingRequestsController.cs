using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Implementations.AssistantServices;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
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

        // GET: PendingRequestsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PendingRequestsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PendingRequestsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PendingRequestsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PendingRequestsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PendingRequestsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PendingRequestsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
