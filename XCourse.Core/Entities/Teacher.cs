using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Teacher
    {
        public int ID { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public bool IsAvilableForPrivateGroup { get; set; }
        public decimal PrivatePrice { get; set; }
        public virtual ICollection<PrivateGroupRequest> PrivateGroupRequests { get; set; } = new HashSet<PrivateGroupRequest>();
        public virtual ICollection<AssistantInvitation> AssistantInvitations { get; set; } = new HashSet<AssistantInvitation>();
        public virtual ICollection<Subject> Subjects { get; set; } = new HashSet<Subject>();
        public virtual ICollection<Group> Groups { get; set; } = new HashSet<Group>();

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
