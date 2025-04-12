using XCourse.Core.Entities;


namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class StudentGroup
    {
        public int GroupId { get; set; }
        public WeekDay DefaultSessionDays { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string TeacherName { get; set; }
        public byte[]? CoverPicture { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public String Subject { get; set; }
    }
}
