namespace XCourse.Core.DTOs.StudentDTOs
{
    public class ReservationDTO
    {
        public int RoomID { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly Start {  get; set; }
        public TimeOnly End { get; set; }
    }
}
