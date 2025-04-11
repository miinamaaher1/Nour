using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Web.Areas.Students.Controllers
{
    [Area("Students")]
    public class CenterController : Controller
    {
        private readonly IMapService _mapService;
        private readonly ICenterReservationService _centerReservationService;

        public CenterController(IMapService mapService, ICenterReservationService centerReservationService)
        {
            _mapService = mapService;
            _centerReservationService = centerReservationService;
        }

        public async Task<IActionResult> Browse()
        {
            var mapInfo = await _mapService.InitializeMapAsync(User);
            return View(mapInfo);
        }

        [HttpGet]
        public IActionResult Search(string query = "")
        {
            var centers = _mapService.SearchCenters(query);
            return Json(centers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var center = await _centerReservationService.GetCenterDetailsAsync(id, User);
            return View(center);
        }

        [HttpPost]
        public IActionResult GetReservations([FromBody] GetReservationsDTO request)
        {
            var result = _centerReservationService.GetDayReservations(request.Id, request.Date);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ReserveRoom([FromBody] ReservationDTO request)
        {
            var response = await _centerReservationService.ReserveRoomAsync(request.RoomID, request.Date, request.Start, request.End, User);
            return Json(response);
        }
    }
}
