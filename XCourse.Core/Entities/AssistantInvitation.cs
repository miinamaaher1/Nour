using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public enum AssistantInvitationStatus
    {
        Pending, 
        Accepted,
        Rejected
    }
    public class AssistantInvitation
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Assistant))]
        [Display(Name = nameof(Assistant))]
        public int AssistantID { get; set; }

        public virtual Assistant? Assistant { get; set; }

        [ForeignKey(nameof(Group))]
        [Display(Name = nameof(Group))]
        public int GroupID { get; set; }
        public AssistantInvitationStatus Status { get; set; }
        public virtual Group? Group { get; set; }
    }
}
