using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XCourse.Core.Entities;

namespace XCourse.Web.Areas.Identity.Pages.Account
{
    public class SelectAccountTypeExternalModel : PageModel
    {
        [BindProperty]
        public AccountType AccountType { get; set; }

        public IActionResult OnPost(AccountType accountType)
        {
            // Save to TempData or Session
            TempData["AccountType"] = accountType;

            return RedirectToPage("/Account/ExternalLogin", "ContinueRegistration", new { accountType });
        }
    }
}