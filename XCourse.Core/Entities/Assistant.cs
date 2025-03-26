using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Assistant
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Display(Name = "App User")]
        public string AppUserID { get; set; }

        public virtual AppUser? AppUser { get; set; }

        public virtual ICollection<AssistantInvitation>? AssistantInvitations { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
