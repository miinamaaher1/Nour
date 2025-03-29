using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Core.Entities
{
    public class GroupDefaults
    {
        public int ID { get; set; }
        public WeekDay? WeekDay { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        [ForeignKey(nameof(Room))]
        public int? RoomID { get; set; }
        public virtual Room? Room { get; set; }
        [ForeignKey(nameof(Group))]
        public int? GroupID { get; set; }
        public virtual Group? Group { get; set; }

    }
}
