using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.DTOs.Teacher
{
    using System.Text.Json.Serialization;

    public class RequestRoomDto
    {
        [JsonPropertyName("centerID")]
        public int CenterID { get; set; }

        [JsonPropertyName("startDate")]
        public DateOnly StartDate { get; set; }

        [JsonPropertyName("startTime")]
        public TimeOnly StartTime { get; set; }

        [JsonPropertyName("endDate")]
        public DateOnly EndDate { get; set; }

        [JsonPropertyName("endTime")]
        public TimeOnly EndTime { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("day")]
        public WeekDay Day { get; set; }
    }

}
