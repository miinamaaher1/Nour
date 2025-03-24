namespace XCourse.Web.Models
{
    public class Attendance
    {
        public int StudentID { get; set; }
        public int SessionId { get; set; }
        public string? Feedback { get; set; }
        public int? Rating { get; set; }
        public bool? HasAttended { get; set; }
        public double? ClassWorkGrade { get; set; }
        public double? HomeWorkGrade { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Session? Session { get; set; }
        // IsDeleted 
        public bool IsDeleted { get; set; }

    }
}
