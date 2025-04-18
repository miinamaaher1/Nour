using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.DTOs.Teachers
{
    public class ReserveOnlineGroupRequestDTO
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int MaxNumberOfStudents { get; set; }
        public decimal PricePerSession { get; set; }
        public bool IsGirlsOnly { get; set; }
        public bool IsPrivate { get; set; }
        public int NumberOfSessionsPerWeek { get; set; }
        public List<DefaultSessionResrvation>? DefaultSessionResrvations { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }

    public class DefaultSessionResrvation
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public WeekDay WeekDay { get; set; } // int enum 1 / 2 / 4 / 8 / 16 / 32 / 64
    }
}
