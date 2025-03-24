using System.ComponentModel.DataAnnotations;
using XCourse.Core.Entities;

namespace XCourse.Core.Entities
{
    public class Wallet
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        public AppUser? User { get; set; }

        public decimal Balance { get; set; } = 0m; // Default balance

        public string Currency { get; set; } = "EGP"; // Store currency if needed

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
