using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class AssistantInvitation
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Assistant))]
        [Display(Name = nameof(Assistant))]
        public int AssistantID { get; set; }

        public virtual Assistant? Assistant { get; set; }

        [ForeignKey(nameof(Teacher))]
        [Display(Name = nameof(Teacher))]
        public int TeacherID { get; set; }

        public virtual Teacher? Teacher { get; set; }

        [ForeignKey(nameof(Group))]
        [Display(Name = nameof(Group))]
        public int GroupID { get; set; }

        public virtual Group? Group { get; set; }
    }
}
