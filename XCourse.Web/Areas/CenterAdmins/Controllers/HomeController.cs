using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Services.Interfaces.CenterAdminServices;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using Microsoft.AspNetCore.Identity;
using XCourse.Infrastructure.Repositories.Interfaces;


namespace XCourse.Web.Areas.CenterAdmins.Controllers
{
    [Area("CenterAdmins")]
    public class HomeController : Controller
    {
        ICenterAdminHomeService CenterAdminHome;
        UserManager<AppUser> _userManager;

        public HomeController(ICenterAdminHomeService centerAdminHomeService, UserManager<AppUser> userManager) 
        {
            CenterAdminHome = centerAdminHomeService;
            _userManager = userManager;
        }


        // GET: HomeController
        public async Task<ActionResult> Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var topCenters = await CenterAdminHome.GetTopCenter(userID);
            var rooms= await CenterAdminHome.GetAllRooms(userID);
            var user = await _userManager.GetUserAsync(User);
            IndexHomeViewModel model = new IndexHomeViewModel
            {
                topCenterViews = topCenters,
                roomHomeViewModels=rooms,
                UserName = user.FirstName + " " + user.LastName
            };
            return View(model);  // This already works with your Index.cshtml
        }




    }
}
