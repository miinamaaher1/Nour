using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    [Flags]
    public enum Equipment
    {
        Study,
        Lecture,
        Meeting
    }
    public class Room
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50,ErrorMessage = "Number of characters for the name must be less than or equal 50")]
        public string Name { get; set; }

        public int Capacity { get; set; }
        public Equipment Equipment { get; set; }

        [Display(Name = "Price per Hour")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerHour { get; set; }

        [ForeignKey(nameof(Center))]
        [Display(Name = nameof(Center))]
        public int CenterID { get; set; }
        public virtual Center? Center { get; set; }
        public virtual ICollection<RoomReservation>? RoomReservations { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
    }
}
