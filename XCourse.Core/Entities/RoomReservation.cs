using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
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

        [ForeignKey(nameof(Session))]
        [Display(Name = nameof(Session))]
        public int? SessionID { get; set; }

        public virtual Session? Session { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsApproved { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
