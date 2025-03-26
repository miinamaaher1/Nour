using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;


namespace XCourse.Core.Entities
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum AccountType
    {
        Student,
        Teacher,
        Assistant,
        CenterAdmin
    }

    public class AppUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Number of characters for first name must be less than or equal 25")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Number of characters for last name must be less than or equal 25")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly? DateOfBirth {get; set;}

        [Display(Name = "Home Address")]
        public Address? HomeAddress { get; set; }

        [Display(Name = "Home Location")]
        [Column(TypeName = "geography")]
        public Point? HomeLocation { get; set; }

        // Wallet
        [ForeignKey(nameof(Wallet))]
        [Display(Name = nameof(Wallet))]
        public int WalletID { get; set; }

        public Wallet? Wallet { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

        // Users Types
        [EnumDataType(typeof(AccountType))]
        public AccountType AccountType { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual CenterAdmin? CenterAdmin { get; set; }
        public virtual Assistant? Assistant { get; set; }
    }
}
