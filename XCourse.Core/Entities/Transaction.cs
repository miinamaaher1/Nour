using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.Entities
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Refund
    }
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; } // Enum for Deposit, Withdrawal, Refund

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string StripePaymentId { get; set; } // Store Stripe transaction ID
    }
}
