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
        public Year Year { get; set; }
        public int TeacherId { get; set; }
        public Semester Semester { get; set; }
    }

}
