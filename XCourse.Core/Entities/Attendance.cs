using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class Attendance
    {
        [ForeignKey(nameof(Student))]
        [Display(Name = nameof(Student))]
        public int StudentID { get; set; }

        [ForeignKey(nameof(Session))]
        [Display(Name = nameof(Session))]
        public int SessionId { get; set; }

        [MaxLength(500, ErrorMessage = "Number of characters for feedback must be less than or equal 500")]
        [Display(Name = nameof(Feedback))]
        public string? Feedback { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public int? Rating { get; set; }

        public bool? HasAttended { get; set; }

        [Display(Name = "Class Work Grade")]
        public double? ClassWorkGrade { get; set; }

        [Display(Name = "Home Work Grade")]
        public double? HomeWorkGrade { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Session? Session { get; set; }
        // IsDeleted 
        public bool IsDeleted { get; set; }
    }
}
