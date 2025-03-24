using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Teacher
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Display(Name = "App User")]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public bool IsAvilableForPrivateGroup { get; set; }

        [Display(Name = "Private Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrivatePrice { get; set; }

        public virtual ICollection<PrivateGroupRequest> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequest>();
        public virtual ICollection<AssistantInvitation> AssistantInvitations { get; set; } = new HashSet<AssistantInvitation>();
        public virtual ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
