using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    using System;
    using System.Collections.Generic;
    using XCourse.Core.Entities;

    public class RequestOfflineGroupReservation
    {
        public int TeacherId { get; set; }
        public string? Governorate { get; set; }
        public Year YearId { get; set; }
        public Semester SemesterId { get; set; }
        public int CenterId { get; set; }
        public int SubjectId { get; set; }
        public int NumberOfSessions { get; set; }
        public int Capacity { get; set; }
        public bool IsGirlsOnly { get; set; }
        public bool IsPrivate { get; set; }
        public decimal PricePerSession { get; set; }
        public List<SessionDetailsRequest> Sessions { get; set; } = new List<SessionDetailsRequest>();
    }

    public class SessionDetailsRequest
    {
        public int RoomId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public WeekDay DayId { get; set; }
    }

}
