using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class CenterAdmin
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Display(Name = "App User")]
        public int AppUserID { get; set; }

        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Center>? Centers { get; set; }

        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
