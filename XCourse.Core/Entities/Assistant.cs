using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Assistant
    {
        public int ID { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<AssistantInvitation> AssistantInvitations { get; set; } = new HashSet<AssistantInvitation>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
