using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.PaymentService;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XCourse.Infrastructure.Repositories.Interfaces;
using AccountType = XCourse.Core.Entities.AccountType;


namespace XCourse.Services.Implementations.PaymentService
{
    public class StripeService : IStripeService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public StripeService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;

        }
        public async Task<string> CreateStripeSessionAsync(decimal amount, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            string successUrl="";
            string cancelUrl="";
            if (user.AccountType == AccountType.Student)
            {
                successUrl = "https://localhost:7054/Students/Payment/StripeSuccess?session_id={CHECKOUT_SESSION_ID}";
                cancelUrl = "https://localhost:7054/Students/Payment/TopUpWallet";
            }
            else if (user.AccountType == AccountType.Teacher)
            {
                successUrl = "https://localhost:7054/Teachers/Payment/StripeSuccess?session_id={CHECKOUT_SESSION_ID}";
                cancelUrl = "https://localhost:7054/Teachers/Payment/TopUpWallet";
            }
            else if (user.AccountType == AccountType.CenterAdmin)
            {
                successUrl = "https://localhost:7054/CenterAdmins/Payment/StripeSuccess?session_id={CHECKOUT_SESSION_ID}";
                cancelUrl = "https://localhost:7054/CenterAdmins/Payment/TopUpWallet";
            }
            else if(user.AccountType==AccountType.Assistant)
            {
                successUrl = "https://localhost:7054/Assistants/Payment/StripeSuccess?session_id={CHECKOUT_SESSION_ID}";
                cancelUrl = "https://localhost:7054/Assistants/Payment/TopUpWallet";
            }

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        UnitAmount = (long)(amount * 100), // Stripe accepts amounts in cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Wallet Top-up"
                        }
                    },
                    Quantity = 1
                }
            },
                    Mode = "payment",

                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl,
                    CustomerEmail = email
                };

            var service = new SessionService();
            Stripe.Checkout.Session session = await service.CreateAsync(options);

            return session.Url;
        }
        public async Task<bool> ProcessStripeSuccessAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            var user = await _userManager.FindByEmailAsync(session.CustomerEmail);
            if (user == null) return false;

            var wallet = await _unitOfWork.Wallets.FindAsync(w => w.AppUserID == user.Id);
            if (wallet == null) return false;

            decimal amount = session.AmountTotal.HasValue ? session.AmountTotal.Value / 100m : 0;

            wallet.Balance += amount;

            _unitOfWork.Transactions.Add(new Transaction
            {
                WalletID = wallet.ID,
                Amount = amount,
                Type = TransactionType.Deposit,
                CreatedAt = DateTime.Now,
                PaymentTransactionID = session.Id
            });

            await _unitOfWork.SaveAsync();
            return true;
        }

      
        
    }
}
