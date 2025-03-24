using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Announcement
    {
        public int ID { get; set; }
        public bool IsImportant { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupID { get; set; }
        public virtual Group? Group { get; set; }
    }
}
