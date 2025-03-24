using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
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
        public virtual ICollection<RoomReservation> RoomReservations { get; set; } = new HashSet<RoomReservation>();
        public virtual ICollection<PrivateGroupRequest> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequest>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
