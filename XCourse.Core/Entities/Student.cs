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
        public string AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }

        [EnumDataType(typeof(Year))]
        public Year? Year { get; set; }

        [EnumDataType(typeof(Major))]
        public Major? Major { get; set; }

        public virtual ICollection<Attendance>? Attendances { get; set; }
        public virtual ICollection<RoomReservation>? RoomReservations { get; set; }
        public virtual ICollection<PrivateGroupRequest>? PrivateGroupRequests { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
