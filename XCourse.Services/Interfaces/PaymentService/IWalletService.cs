using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.PaymentViewModels;

namespace XCourse.Services.Interfaces.PaymentService
{
    public interface IWalletService
    {
        public Task<PaymentDetailsVM> GetPaymentDetailsAsync(ClaimsPrincipal claimsPrincipal);

    }
}
