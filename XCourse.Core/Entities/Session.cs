using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Session
    {
        public int ID { get; set; }
        public TimeSpan Duration { get; set; }
        public Location? Location { get; set; }
        public Address? Address { get; set; }
        public bool IsOnline { get; set; }
        public string URL { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        [ForeignKey(nameof(RoomReservation))]
        public int RoomReservationID { get; set; }
        public virtual RoomReservation? RoomReservation { get; set; }
        [ForeignKey(nameof(Group))]
        public int GroupID { get; set; }
        public virtual Group? Group { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
