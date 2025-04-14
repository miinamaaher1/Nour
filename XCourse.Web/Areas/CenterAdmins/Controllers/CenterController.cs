using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using XCourse.Services.Interfaces.CenterAdminServices;

namespace XCourse.Web.Areas.CenterAdmins.Controllers
{
    [Area("CenterAdmins")]
    public class CenterController : Controller
    {
        // GET: CenterController
        ICenterAdminService _centerAdminService;
        IConfiguration Configuration;
        public CenterController(ICenterAdminService centerAdminService,IConfiguration _configuration)
        {
            _centerAdminService = centerAdminService;
            Configuration = _configuration;
        }

        public ActionResult Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userID))
                return Unauthorized("User ID not found");
            var Centers=_centerAdminService.GetCenters(userID);
            if (Centers == null)
            {
                 return NotFound("this User not Have Any Center");
            }
            ViewBag.centeradmin = Centers.Select(c => c.CenterAdminid).FirstOrDefault();
        
            return View(Centers);
        }

        // GET: CenterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CenterController/Create
        public ActionResult Create(int centeradminid)
        {

            var model = new CreateCenterViewModel
            {
                CenterAdminid = centeradminid,
                Location = new MapInfoDTO { OriginX = 0, OriginY = 0, Key = Configuration["GoogleMaps:ApiKey"] }
            };
            return View(model);
          
        }

        // POST: CenterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( CreateCenterViewModel Center)
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();

                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    Center.PreviewPicture= dataStream.ToArray();
                }
            }
            if (Center.Location == null)
            {
                Center.Location = new MapInfoDTO
                {
                    OriginX = 0,
                    OriginY = 0,
                    Key = Configuration["GoogleMaps:ApiKey"]
                };
            }
            else if (string.IsNullOrEmpty(Center.Location.Key))
            {
                Center.Location.Key = Configuration["GoogleMaps:ApiKey"];
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _centerAdminService.AddNewCenter(Center);
                    return RedirectToAction(nameof(Index));
                }

                return View(Center);
            }
            catch
            {
                return View(Center);
            }
        }



        public ActionResult GetRooms(int id)
        {
         var rooms=   _centerAdminService.getAllRooms(id);
            if(rooms==null)
            {
                return NotFound();
            }

            return View(rooms);
        }




        // GET: CenterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CenterController/Edit/5
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

        // GET: CenterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CenterController/Delete/5
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
