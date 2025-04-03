using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class CenterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapService _mapService;

        public CenterController(UserManager<AppUser> userManager, IMapService mapService)
        {
            _userManager = userManager;
            _mapService = mapService;
        }

        async public Task<IActionResult> Browse()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var mapInfo = _mapService.InitializeMap(user);
            return View(mapInfo);
        }
    }
}
