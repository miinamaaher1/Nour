using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using XCourse.Services.Interfaces.CenterAdminServices;

namespace XCourse.Web.Areas.CenterAdmins.Controllers
{
    [Area("CenterAdmins")]
    public class ReservationController : Controller
    {
        ICenterAdminService _centerAdminService;


        public ReservationController(ICenterAdminService centerAdminService)
        {
            _centerAdminService = centerAdminService;

        }
        // GET: ReservationController

        public ActionResult IndexForReservations(int id)
        {
            var Reservations = _centerAdminService.GetReservations(id);
            if (Reservations == null) return NotFound();

            return View(Reservations);
        }
        public ActionResult AcceptReservation(int id)
        {
            int t = _centerAdminService.ApproveReservation(id);
            if (t == 0)
            {
                return NotFound();
            }
            return RedirectToAction("IndexForReservations", new { id = t });

        }



        public ActionResult RejectReservation(int id)
        {
            int t = _centerAdminService.RejectReservation(id);
            if (t == 0)
            {
                return NotFound();
            }
            return RedirectToAction("IndexForReservations", new { id = t });

        }
        // GET: ReservationController/Details/5
        public ActionResult DetailsReservation(int id)
        {
            var model = _centerAdminService.DetailsReservation(id);


            if (model == null) return NotFound();



            return View(model);
        }


    
        // GET: ReservationController/Edit/5
        public ActionResult EditReservation(int id)
        {
       var Result=     _centerAdminService.EditRoomReservation(id);
            if(Result==null)
                return NotFound();
            return View(Result);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReservation(EditRoomReservation editRoom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int t = _centerAdminService.UpdateReservtion(editRoom);
                    if (t == 0) return NotFound();

                    return RedirectToAction("IndexForReservations", new { id = t });
                }
                else
                {
                   var Rooms = _centerAdminService.EditRoomReservation(editRoom.ID);
                    return View(Rooms);
                }

            }
            catch
            {
                var Rooms = _centerAdminService.EditRoomReservation(editRoom.ID);
                return View(Rooms);
            }
        }

        // GET: ReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservationController/Delete/5
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
