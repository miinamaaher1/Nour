using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    public class CenterAdmin
    {
        public int ID { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int AppUserID { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Center> Centers { get; set; } = new HashSet<Center>();

        // IsDeleted 
        public bool IsDeleted { get; set; }

    }
}
