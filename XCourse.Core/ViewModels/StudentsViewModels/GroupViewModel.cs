namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class GroupViewModel
    {
        public int GroupID { get; set; }
        public string GroupTeacherName { get; set; }
        public string SubjectName { get; set; }
        public decimal PricePerSession { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudentsCount { get; set; }
        public byte[] TeacherProfilePicture { get; set; }
        public byte[] GroupPicture { get; set; }
    }
}
