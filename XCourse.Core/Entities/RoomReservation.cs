using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public enum ReservationStatus
    {
        Pending,
        Declined,
        Approved
    }
    public class RoomReservation
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Room))]
        [Display(Name = nameof(Room))]
        public int RoomID { get; set; }
        public virtual Room? Room { get; set; }

        [ForeignKey(nameof(Student))]
        [Display(Name = nameof(Student))]
        public int? StudentID { get; set; }
        public virtual Student? Student { get; set; }

        public virtual Session? Session { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public WeekDay? WeekDay { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
