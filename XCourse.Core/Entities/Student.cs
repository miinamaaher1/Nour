using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Display(Name = "App User")]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }

        [EnumDataType(typeof(Year))]
        public Year Year { get; set; }

        [MaxLength(25,ErrorMessage = "Number of characters for major must be less than or equal 25")]
        public string? Major { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
        public virtual ICollection<RoomReservation> RoomReservations { get; set; } = new HashSet<RoomReservation>();
        public virtual ICollection<PrivateGroupRequest> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequest>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
