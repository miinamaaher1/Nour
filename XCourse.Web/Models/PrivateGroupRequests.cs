using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class PrivateGroupRequests
    {
        [ForeignKey(nameof(Student))]
        public int StudentID { get; set; }
        public virtual Student? Student { get; set; }

        [ForeignKey(nameof(Teacher))]
        public int TeacherID { get; set; }
        public virtual Teacher? Teacher { get; set; }

        public bool IsApproved { get; set; } 
        public Address Address { get; set; }
        public Location Location { get; set; }
        public string? Details { get; set; }
        public Subject Subject { get; set; }
    }
}
