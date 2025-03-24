using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class Student
    {
        public int ID { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public Year Year { get; set; }
        public string? Major { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
        public virtual ICollection<RoomReservation> RoomReservation { get; set; } = new HashSet<RoomReservation>();
        public virtual ICollection<PrivateGroupRequests> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequests>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
