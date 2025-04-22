using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using XCourse.Services.Interfaces.CenterAdminServices;

namespace XCourse.Web.Areas.CenterAdmins.Controllers
{
    [Authorize(Roles = "CenterAdmin")]
    [Area("CenterAdmins")]
    public class CenterController : Controller
    {
        // GET: CenterController
        ICenterAdminService _centerAdminService;
        IConfiguration Configuration;
        public CenterController(ICenterAdminService centerAdminService, IConfiguration _configuration)
        {
            _centerAdminService = centerAdminService;
            Configuration = _configuration;
        }

        public ActionResult Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userID))
                return Unauthorized("User ID not found");
            var Centers = _centerAdminService.GetCenters(userID);
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
            var Center = _centerAdminService.GetCenter(id);
            if (Center == null) return NotFound();

            return View(Center);
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
        public async Task<ActionResult> Create(CreateCenterViewModel Center)
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();

                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    Center.PreviewPicture = dataStream.ToArray();
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








        // GET: CenterController/Edit/5
        public ActionResult Edit(int id)
        {
            var EditCenter = _centerAdminService.GetCenter(id);
            if (EditCenter == null) return NotFound();


            return View(EditCenter);
        }

        // POST: CenterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateCenterViewModel EditCenter)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();

                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        EditCenter.PreviewPicture = dataStream.ToArray();
                    }
                }
                if (ModelState.IsValid)
                {
                    int t = _centerAdminService.EditCenter(EditCenter);
                    if (t == 1)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    else return View(EditCenter);
                }


                return View(EditCenter);
            }
            catch
            {
                return View(EditCenter);
            }
        }

        // GET: CenterController/Delete/5
        public ActionResult Delete(int id)
        {
            var deletecenter = _centerAdminService.GetCenter(id);
            if (deletecenter == null) return NotFound();
            return View(deletecenter);
        }

        // POST: CenterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CreateCenterViewModel Center)
        {
            try
            {


                int t = _centerAdminService.DeleteCenter(Center);
                if (t == 1)
                {
                    return RedirectToAction(nameof(Index));

                }

                else
                {


                    return RedirectToAction("ConfirmDelete", new { CenterID = Center.CenterID });

                }

            }
            catch
            {
                return View(Center);
            }
        }




        public ActionResult ConfirmDelete(int CenterID)
        {
            var Deletemessage = _centerAdminService.GetCenter(CenterID);
            return View(Deletemessage);

        }

    }
}
