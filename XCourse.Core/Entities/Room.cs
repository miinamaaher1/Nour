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
        public int ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Equipment Equipment { get; set; }
        public decimal PricePerHour { get; set; }
        [ForeignKey(nameof(Center))]
        public int CenterID { get; set; }
        public virtual Center? Center { get; set; }
        public virtual ICollection<RoomReservation> RoomReservations { get; set; } = new HashSet<RoomReservation>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();
    }
}
