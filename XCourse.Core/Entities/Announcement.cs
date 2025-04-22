using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.Entities
{
    public class Announcement
    {
        [Key]
        public int ID { get; set; }

        public bool IsImportant { get; set; }

        [MaxLength(50, ErrorMessage = "Number of characters for Title must be less than or equal 50")]
        public string? Title { get; set; }

        [MaxLength(300, ErrorMessage = "Number of characters for Body must be less than or equal 300")]
        public string? Body { get; set; }

        [DataType(dataType: DataType.DateTime)]
        public DateTime? DateTime { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
    }
}
