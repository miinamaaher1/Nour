using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class PrivateGroupRequest
    {
        [ForeignKey(nameof(Student))]
        public int StudentID { get; set; }
        public virtual Student? Student { get; set; }

        [ForeignKey(nameof(Teacher))]
        public int TeacherID { get; set; }
        public virtual Teacher? Teacher { get; set; }
        [ForeignKey(nameof(Subject))]
        public int SubjectID { get; set; }
        public virtual Subject? Subject { get; set; }

        public bool IsApproved { get; set; } 
        public Address Address { get; set; }
        public Location Location { get; set; }
        public string? Details { get; set; }
    }
}
