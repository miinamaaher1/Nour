namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class TeacherProfileVM
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public byte[] TeacherProfilePicture { get; set; }
        public List<CompactGroupVM> AvailbleGroups { get; set; }
    }
}
