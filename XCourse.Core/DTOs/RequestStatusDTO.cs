namespace XCourse.Core.DTOs
{
    public class RequestStatusDTO
    {
        public bool IsValid { get; set; }
        public List<string>? Errors { get; set; }
    }
}
