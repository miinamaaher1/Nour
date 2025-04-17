using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.PaymentService;

namespace XCourse.Services.Implementations.PaymentService
{
    public class TransactionService:ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public TransactionService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> MakeTransactionAsync(ClaimsPrincipal claimsPrincipal, string appUserReceiverId, decimal amount ,TransactionType transactionType)
        {
            //sender's wallet
            var sender = await _userManager.GetUserAsync(claimsPrincipal);
            if (sender == null)
            {
                return false;
            }
            var senderWallet = await _unitOfWork.Wallets.FindAsync(w => w.AppUserID == sender.Id);
            //receiver's wallet
            var receiver = await _userManager.FindByIdAsync(appUserReceiverId);
            if (receiver == null)
            {
                return false;
            }
            var receiverWallet = await _unitOfWork.Wallets.FindAsync(w => w.AppUserID == receiver.Id);
            if (senderWallet == null || receiverWallet == null || senderWallet.Balance < amount)
            {
                return false;
            }

            senderWallet.Balance -= amount;
            receiverWallet.Balance += amount;
            _unitOfWork.Transactions.Add(new Transaction
            {
                WalletID = senderWallet.ID,
                Amount = -amount,
                CreatedAt = DateTime.Now,
                Type = transactionType,
                PaymentTransactionID = Guid.NewGuid().ToString(),
            });
            _unitOfWork.Transactions.Add(new Transaction
            {
                WalletID = receiverWallet.ID,
                Amount = amount,
                CreatedAt = DateTime.Now,
                Type = transactionType,
                PaymentTransactionID = Guid.NewGuid().ToString()
            });
            await _unitOfWork.SaveAsync();
            return true;
        }


    }
}
