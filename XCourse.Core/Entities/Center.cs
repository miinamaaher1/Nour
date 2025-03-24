using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;


namespace XCourse.Core.Entities
{
    public class Center
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(80, ErrorMessage = "Number of characters for center's name must be less than or equal 80")]
        [Display(Name = "Center's Name")]
        public string Name { get; set; }

        public bool IsGirlsOnly { get; set; }

        [Column(TypeName = "geography")]
        public Point Location { get; set; }

        public Address Address { get; set; }

        [ForeignKey(nameof(CenterAdmin))]
        [Display(Name = "Center Admin")]
        public int CenterAdminID { get; set; }

        public virtual CenterAdmin? CenterAdmin { get; set; }
        public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
