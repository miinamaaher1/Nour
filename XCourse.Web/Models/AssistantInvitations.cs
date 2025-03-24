using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class AssistantInvitations
    {
        public int ID { get; set; }
        [ForeignKey(nameof(Assistant))]
        public int AssistantID { get; set; }
        public virtual Assistant? Assistant { get; set; }
        [ForeignKey(nameof(Teacher))]
        public int TeacherID { get; set; }
        public virtual Teacher? Teacher { get; set; }
        [ForeignKey(nameof(Group))]
        public int GroupID { get; set; }
        public virtual Group? Group { get; set; }
    }
}
