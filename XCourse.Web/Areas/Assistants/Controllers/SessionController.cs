using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Area("Assistants")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            this._sessionService = sessionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        async public Task<IActionResult> Details(int id)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int assistantId = 0;
            if (userID != null)
            {
                Assistant assistant = await _sessionService.GetAssistantByUserId(userID);
                assistantId = assistant.ID;
            }
            var session = await _sessionService.GetSessionDetailsById(id, assistantId);
            return View(session);
        }
    }
}
