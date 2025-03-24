using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class Assistant
    {
        public int ID { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<AssistantInvitations> AssistantInvitations { get; set; } = new HashSet<AssistantInvitations>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
