using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class AppUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth {get; set;}
        public Address HomeAddress { get; set; }
        public Location HomeLocation { get; set; }

        // Wallet
        [ForeignKey(nameof(Wallet))]
        public int WalletID { get; set; }
        public Wallet? Wallet { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

        // Users Types
        // 1. Student
        [ForeignKey(nameof(Student))]
        public int? StudentID {get; set;}
        public virtual Student? Student { get; set; }

        // 2. Teacher
        [ForeignKey(nameof(Teacher))]
        public int? TeacherID { get; set; }
        public virtual Teacher? Teacher { get; set; }

        // 3. CenterAdimn
        [ForeignKey(nameof(CenterAdmin))]
        public int? CenterAdminID { get; set; }
        public virtual CenterAdmin? CenterAdmin { get; set; }

        // 4. Assistant
        [ForeignKey(nameof(Assistant))]
        public int? AssistantID { get; set; }
        public virtual Assistant? Assistant { get; set; }


    }
}
