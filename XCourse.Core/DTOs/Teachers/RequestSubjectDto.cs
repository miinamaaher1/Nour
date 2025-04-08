using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.DTOs.Teachers
{
    public class RequestSubjectDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Year Year { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Semester Semester { get; set; }
    }

}
