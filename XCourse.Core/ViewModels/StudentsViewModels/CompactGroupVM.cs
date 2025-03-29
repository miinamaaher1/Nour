using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class CompactGroupVM
    {
        public int GroupID { get; set; }
        public string SubjectName { get; set; }
        public Semester Semester { get; set; }
        public Year Year { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public decimal PricePerSession { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public byte[]? CoverPicture { get; set; }
    }
}
