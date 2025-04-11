using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class AnnouncementVM
    {
        public int ID { get; set; }

        public bool IsImportant { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public DateTime? DateTime { get; set; }
        public List<AnnouncementGroup>? GroupSubjectNames { get; set; }
    }


    public class AnnouncementGroup
    {
        public int GroupID { get; set; }
        public string GroupSubject { get; set; } = string.Empty;

    }
}
