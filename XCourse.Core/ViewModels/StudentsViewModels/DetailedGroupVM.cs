using NetTopologySuite.Geometries;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class DetailedGroupVM
    {
        public int ID { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public decimal PricePerSession { get; set; }
        public ICollection<DefaultTimeVM> DefaultTimes { get; set; }
        public string? CenterName { get; set; }
        public Point? Location { get; set; }
        public string MapKey { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string TeacherName { get; set; }
        public byte[]? TeacherProfilePicture { get; set; }
        public byte[]? CoverPicture { get; set; }
        public string SubjectName { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }
        public Major Major { get; set; }
        public int StudentID { get; set; }
        public Gender StudentGender { get; set; }
    }
}
