using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.PaymentViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.PaymentService;

namespace XCourse.Services.Implementations.PaymentService
{
    public class WalletService:IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public WalletService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<PaymentDetailsVM> GetPaymentDetailsAsync( ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);


            
            if (user == null)
            {
                return new PaymentDetailsVM();
            }
            var wallet = await _unitOfWork.Wallets.FindAsync(w => w.AppUserID == user.Id);
            if (wallet == null)
            {
                return new PaymentDetailsVM();
            }
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var transactions = await _unitOfWork.Transactions.FindAllAsync(
                t => t.WalletID == wallet.ID && t.CreatedAt >= thirtyDaysAgo
            );
            var paymentDetails = new PaymentDetailsVM
            {
                UserName = user.FirstName+" "+user.LastName,
                Balance = wallet.Balance,
                PaymentHistory = transactions.Select(t => new PaymentHistoryVM
                {
                    Amount = t.Amount,
                    Date = t.CreatedAt,
                    Status = t.Type
                }).ToList()
            };
            return paymentDetails;
        }
    }
}
