using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Services.Interfaces.CenterAdminServices;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels;
using XCourse.Core.ViewModels.CenterAdminViewModels;


namespace XCourse.Web.Areas.CenterAdmins.Controllers
{
    [Area("CenterAdmins")]
    public class HomeController : Controller
    {
        ICenterAdminHomeService CenterAdminHome;

        public HomeController(ICenterAdminHomeService centerAdminHomeService) {
        
        CenterAdminHome = centerAdminHomeService;
        }


        // GET: HomeController
        public async Task<ActionResult> Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var topCenters = await CenterAdminHome.GetTopCenter(userID);
            var rooms= await CenterAdminHome.GetAllRooms(userID);
            IndexHomeViewModel model = new IndexHomeViewModel
            {
                topCenterViews = topCenters,
                roomHomeViewModels=rooms
            };
            return View(model);  // This already works with your Index.cshtml
        }




    }
}
