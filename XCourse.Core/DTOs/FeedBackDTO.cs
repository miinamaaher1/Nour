using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs
{
    public class FeedBackDTO
    {
        public int StudentId { get; set; }
        public int SessionID { get; set; }
        public string? Feedback { get; set; }
        public int? Rating { get; set; }
    }
}
