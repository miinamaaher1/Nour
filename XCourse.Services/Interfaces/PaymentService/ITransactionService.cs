using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.PaymentService
{
    public interface ITransactionService
    {
        public Task<bool> MakeTransactionAsync(string userIdSender, string appUserReceiverId, decimal amount, TransactionType transactionType);

    }
}
