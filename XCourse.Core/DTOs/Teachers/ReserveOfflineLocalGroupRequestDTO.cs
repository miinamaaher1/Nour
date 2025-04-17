using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.DTOs.Teachers
{
    public class ReserveOfflineLocalGroupRequestDTO
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int MaxNumberOfStudents { get; set; }
        public decimal PricePerSession { get; set; }
        public string? Location { get; set; }
        public string? Governorate { get; set; }
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
        public string? Street { get; set; }
        public bool IsGirlsOnly { get; set; }
        public bool IsPrivate { get; set; }
        public int NumberOfSessionsPerWeek { get; set; }
        public List<DefaultSessionResrvation>? DefaultSessionResrvations { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }

}
