using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.PaymentService;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Area("Teachers")]
    public class PaymentController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly IWalletService _walletService;
        private readonly UserManager<AppUser> _userManager;

        public PaymentController(IStripeService stripeService, UserManager<AppUser> userManager, IWalletService walletService)
        {
            _stripeService = stripeService;
            _walletService = walletService;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> TopUpWallet()
        {
            var paymentDetailsVm = await _walletService.GetPaymentDetailsAsync(User);
            return View(paymentDetailsVm);
        }

        [HttpPost]
        public async Task<ActionResult> TopUpWallet(decimal amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            string userEmail = user.Email;
            string sessionUrl = await _stripeService.CreateStripeSessionAsync(amount, userEmail);
            return Redirect(sessionUrl);
        }

        public async Task<ActionResult> StripeSuccess(string session_id)
        {
            var result = await _stripeService.ProcessStripeSuccessAsync(session_id);

            var paymentDetailsVm = await _walletService.GetPaymentDetailsAsync(User);
            if (!result)
            {
                paymentDetailsVm.Toast = "error";
                return RedirectToAction("TopUpWallet", paymentDetailsVm);

            }
            else
            {
                paymentDetailsVm.Toast = "success";
                return RedirectToAction("TopUpWallet", paymentDetailsVm);
            }

        }
    }
}
