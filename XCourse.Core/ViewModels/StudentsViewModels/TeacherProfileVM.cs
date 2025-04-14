namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class TeacherProfileVM
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string TagLine { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherPhone { get; set; }
        public bool IsAvailableForPrivateGroups { get; set; }
        public decimal? PrivateGroupPrice { get; set; }
        public byte[]? TeacherProfilePicture { get; set; }
        public List<CompactGroupVM> AvailbleGroups { get; set; }
        public int StudentID { get; set; }
    }
}
