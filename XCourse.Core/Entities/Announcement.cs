using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Announcement
    {
        [Key]
        public int ID { get; set; }

        public bool IsImportant { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Number of characters for Title must be less than or equal 50")]
        public string Title { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage = "Number of characters for Body must be less than or equal 300")]
        public string Body { get; set; }

        [Required]
        [DataType(dataType: DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Display(Name = nameof(Group))]
        [ForeignKey(nameof(Group))]
        public int GroupID { get; set; }
        public virtual Group? Group { get; set; }
    }
}
