namespace XCourse.Core.DTOs.Teachers
{
    public class SessionAttendanceDTO
    {
        public bool IsValid { get; set; }
        public virtual List<String>? Errors { get; set; } = new List<string>();
        public int SessionId { get; set; }
        public int TeacherId { get; set; }
        public virtual List<SessionDayAttendance>? SessionDayAttendances { get; set; }
    }
    public class SessionDayAttendance
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? studentEmail { get; set; }
        public int SessionId { get; set; }
        public bool? HasAttended { get; set; }
        public bool HasPaid { get; set; }
        public double? ClassWorkGrade { get; set; }
        public double? HomeWorkGrade { get; set; }
    }
}