using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class Group
    {
        public int ID { get; set; }
        public int MaxStudents { get; set; }
        public int PricePerSession { get; set; }
        public Address Address { get; set; }
        public Location Location { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        [ForeignKey(nameof(Teacher))]
        public int TeacherID { get; set; }
        public virtual Teacher? Student { get; set; }
        [ForeignKey(nameof(Room))]
        public int? DefaultRoomID { get; set; }
        public virtual Room? DefaultRoom { get; set; }
        [ForeignKey(nameof(Subject))]
        public int? SubjectID { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<AssistantInvitations> AssistantInvitationss { get; set; } = new HashSet<AssistantInvitations>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<Announcement> Announcements { get; set; } = new HashSet<Announcement>();
        public virtual ICollection<Session> Sessions { get; set; } = new HashSet<Session>();


        // IsDeleted 
        public bool IsDeleted { get; set; }

    }
}
