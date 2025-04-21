using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Algorithm;
using XCourse.Core.DTOs.CenterAdmins;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Services.Interfaces.CenterAdminServices;

namespace XCourse.Web.Areas.CenterAdmins.Controllers
{
    [Area("CenterAdmins")]
    public class RoomController : Controller
    {

        ICenterAdminService _centerAdminService;
       
        public RoomController(ICenterAdminService centerAdminService )
        {
            _centerAdminService = centerAdminService;
           
        }
        // GET: RoomController
        public ActionResult Index(int id)
        {
            var rooms = _centerAdminService.getAllRooms(id);
            if (rooms == null)
            {
                return NotFound();
            }
            ViewBag.CenterID = id;
            return View(rooms);
        }

        // GET: RoomController/Create
        public ActionResult Create(int id)
        {
            var room = new RoomDto()
            {
                CenterId= id
            };

                return View(room);
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoomDto room)
        {
           
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        room.PreviewPicture2 = dataStream.ToArray();
                    }
                }

                if (ModelState.IsValid)
                {
                   
                    int t= _centerAdminService.AddNewRoom(room);
                    if(t==1)
                    {
                        return RedirectToAction("Index", new { id = room.CenterId });
                    }
                    else
                    {
                        return View(room);
                    }
                }
                return View(room);
                
            }
            catch
            {
                return View(room);
            }
        }


        // GET: RoomController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _centerAdminService.GetRoom(id);

         
            model.SelectedEquipments = Enum.GetValues(typeof(Equipment))
              .Cast<Equipment>()
            .Where(e => (model.Equipment & e) == e)
           .ToList();
            


            return View(model);
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoomDto room)
        {

            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        room.PreviewPicture2 = dataStream.ToArray();
                    }
                }
                if (ModelState.IsValid)
                {
                    

                    int t= _centerAdminService.EditRoom(room);
                    if (t == 1)
                    {
                        return RedirectToAction("Index", new { id = room.CenterId });
                    }
                    else
                        {
                        
                        return View(room);
                    
                    }
                }
                return View(room);
            }
            catch
            {
                return View(room);
            }
        }


     

        
       


       

       
        // GET: RoomController/Delete/5
        public ActionResult Delete(int id)
        {
            var room = _centerAdminService.GetRoom(id);
            if (room == null) { return View("Error"); }
            return View(room);
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RoomDto room)
        {
           
                try
                {

                    int t = _centerAdminService.DeleteRoom(room);
                    if (t == 1)
                    {
                        return RedirectToAction("Index", new { id = room.CenterId });
                    }
                    else
                {
                    
                        return RedirectToAction("TransfomReservations", new { RoomId=room.RoomId,  CenterId=room.CenterId });

                    }
                }
                catch
                {
                    return View(room);
                }
            

           

        }


       
        public ActionResult TransfomReservations(int RoomId, int CenterId)
        {

            var Avalible = _centerAdminService.transfomReservations(RoomId,CenterId);
            if (Avalible.ApproveReservations.All(r => r.AvailableRooms.Count == 0))
            {
                return RedirectToAction("UnableDeleting", new { id = RoomId });

            }


            return View (Avalible);



        }

        [HttpPost]
        public ActionResult TransfomReservations(transfomReservations transfom)
        {
            try
            {
                int t = _centerAdminService.ConfirmTransformRoom(transfom);

                if(t == 1)
                {
                    return RedirectToAction("Index", new { id = transfom.CenterId });
                }

                else
                {
                    return View(transfom);
                        
                       


                }
            }

            catch
            {
                return View(transfom);
            }




        }


        public ActionResult UnableDeleting(int id)
        {


            var Deletemessage = _centerAdminService.GetRoom(id);
            return View(Deletemessage);


        }





    }
}
