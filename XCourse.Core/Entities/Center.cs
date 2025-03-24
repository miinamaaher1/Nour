using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Center
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsGirlsOnly { get; set; }
        public Location Location { get; set; }
        public Address Address { get; set; }
        [ForeignKey(nameof(CenterAdmin))]
        public int CenterAdminID { get; set; }

        public virtual CenterAdmin? CenterAdmin { get; set; }
        public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
