using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class RoomReservation
    {
        public int ID { get; set; }
        [ForeignKey(nameof(Room))]
        public int RoomID { get; set; }
        public virtual Room? Room { get; set; }

        [ForeignKey(nameof(Student))]
        public int? StudentID { get; set; }
        public virtual Student? Student { get; set; }

        [ForeignKey(nameof(Session))]
        public int? SessionID { get; set; }
        public virtual Session? Session { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsApproved { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
