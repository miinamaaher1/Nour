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

        public ActionResult Index(int id)
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
            return RedirectToAction("Index", new { id = t });
        }

        public ActionResult RejectReservation(int id)
        {
            int t = _centerAdminService.RejectReservation(id);
            if (t == 0)
            {
                return NotFound();
            }
            return RedirectToAction("Index", new { id = t });
        }

        // GET: ReservationController/Edit/5
        public ActionResult Edit(int id)
        {
       var Result = _centerAdminService.EditRoomReservation(id);
            if(Result==null)
                return NotFound();
            return View(Result);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRoomReservation editRoom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int t = _centerAdminService.UpdateReservtion(editRoom);
                    if (t == 0) return NotFound();

                    return RedirectToAction("Index", new { id = t });
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


        public ActionResult Delete(int id)
        {
            var Result = _centerAdminService.DetailsReservation(id);
            if (Result == null)
                return NotFound();
            return View(Result);
        }
        [HttpPost]
        public ActionResult Delete(DetailsReservationViewModel details)
        {
            var deleteReservation = _centerAdminService.DeleteReservation(details);

            try
            {
                if (deleteReservation == null)
                    return NotFound();
               if(deleteReservation==0)
                {
                    return View(details);
                }
                return RedirectToAction("Index", new { id = deleteReservation });

            }


            catch
            {
                return View(details);
            }


           
        }



    }
}