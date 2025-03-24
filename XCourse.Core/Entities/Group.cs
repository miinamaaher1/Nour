using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Group
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Maximum no. of Students")]
        public int MaxStudents { get; set; }

        [Display(Name = "Price per Session")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerSession { get; set; }

        public Address Address { get; set; }

        [Column(TypeName = "geography")]
        public Point Location { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }

        [ForeignKey(nameof(Teacher))]
        [Display(Name = nameof(Teacher))]
        public int TeacherID { get; set; }

        public virtual Teacher? Teacher { get; set; }

        [ForeignKey(nameof(Room))]
        [Display(Name = nameof(Room))]
        public int? DefaultRoomID { get; set; }

        public virtual Room? DefaultRoom { get; set; }

        [ForeignKey(nameof(Subject))]
        [Display(Name = nameof(Subject))]
        public int? SubjectID { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<AssistantInvitation> AssistantInvitations { get; set; } = new HashSet<AssistantInvitation>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<Announcement> Announcements { get; set; } = new HashSet<Announcement>();
        public virtual ICollection<Session> Sessions { get; set; } = new HashSet<Session>();


        // IsDeleted 
        public bool IsDeleted { get; set; }

    }
}
