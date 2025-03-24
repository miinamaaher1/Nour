using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey(nameof(Wallet))]
        [Display(Name = "Wallet")]
        public int WalletID { get; set; }
        public virtual Wallet? Wallet { get; set; }

        public decimal Amount { get; set; }

        [EnumDataType(typeof(TransactionType))]
        public TransactionType Type { get; set; } // Enum for Deposit, Withdrawal, Refund

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string StripePaymentId { get; set; } // Store Stripe transaction ID
    }
}
