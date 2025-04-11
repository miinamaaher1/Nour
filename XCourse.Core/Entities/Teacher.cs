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
        public string AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public string? TagLine { get; set; }
        public bool IsAvailableForPrivateGroups { get; set; }

        [Display(Name = "Private Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrivatePrice { get; set; }

        public virtual ICollection<PrivateGroupRequest>? PrivateGroupRequests { get; set; }
        public virtual ICollection<AssistantInvitation>? AssistantInvitations { get; set; }
        public virtual ICollection<Subject>? Subjects { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
