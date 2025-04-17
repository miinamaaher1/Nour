using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace XCourse.Core.Entities
{
    public class Session
    {
        [Key]
        public int ID { get; set; }

        public TimeSpan? Duration { get; set; }

        [Column(TypeName = "geography")]
        public Point? Location { get; set; }

        public Address? Address { get; set; }

        public bool IsOnline { get; set; }
        public string? Description { get; set; }
        [Url]
        public string? URL { get; set; } // for online groups [optional]
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        [ForeignKey(nameof(RoomReservation))]
        [Display(Name = "Room Reservation")]
        public int? RoomReservationID { get; set; }
        public virtual RoomReservation? RoomReservation { get; set; }

        [ForeignKey(nameof(Group))]
        [Display(Name = nameof(Group))]
        public int GroupID { get; set; }
        public virtual Group? Group { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
