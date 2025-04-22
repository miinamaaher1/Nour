using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Web.Areas.Assistants.Controllers
{
    [Authorize(Roles = "Assistant")]
    [Area("Assistants")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IConfiguration _configuration;
        public SessionController(ISessionService sessionService, IConfiguration configuration)
        {
            _sessionService = sessionService;
            _configuration = configuration;
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
            ViewBag.Key = _configuration["GoogleMaps:ApiKey"];
            return View(session);
        }
    }
}
