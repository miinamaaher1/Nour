using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs;

namespace XCourse.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(ErrorStatusDTO? errorStatusDTO = null)
        {
            // Ensure default values if null
            errorStatusDTO ??= new ErrorStatusDTO();
            

            return View("Error", errorStatusDTO);
        }
    }

}
