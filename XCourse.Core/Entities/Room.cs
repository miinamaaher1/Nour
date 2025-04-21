using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    [Flags]
    public enum Equipment
    {
        None = 0,
        Study = 1,    // 0001
        Lecture = 2,    // 0010
        Meeting = 4
    }
    public class Room
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50,ErrorMessage = "Number of characters for the name must be less than or equal 50")]
        public string Name { get; set; }

        public int Capacity { get; set; }
        [EnumDataType(typeof(Equipment))]
        public Equipment Equipment { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Price per Hour")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerHour { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        [Display(Name = "Preview Picture")]
        public byte[]? PreviewPicture { get; set; }

        [ForeignKey(nameof(Center))]
        [Display(Name = nameof(Center))]
        public int CenterID { get; set; }
        public virtual Center? Center { get; set; }
        public virtual ICollection<RoomReservation>? RoomReservations { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }


    }
}
