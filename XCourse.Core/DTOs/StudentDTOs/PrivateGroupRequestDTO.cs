using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.DTOs.StudentDTOs
{
    public class PrivateGroupRequestDTO
    {
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }
        [MaxLength(25)]
        public string? Street { get; set; }
        [MaxLength(25)]
        public string? Neighborhood { get; set; }
        [MaxLength(25)]
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? Details { get; set; }

    }
}
