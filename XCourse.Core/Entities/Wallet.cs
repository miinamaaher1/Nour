using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XCourse.Core.Entities;

namespace XCourse.Core.Entities
{
    public class Wallet
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Display(Name = "App User")]
        public string AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0m; // Default balance


        public string Currency { get; set; } = "EGP"; // Store currency if needed

        public ICollection<Transaction>? Transactions { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
