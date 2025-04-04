using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    public class RequestAnnouncement
    {
        public int groupId { get; set; }
        public string? title { get; set; }
        public string body { get; set; }
        public bool isImportant {get; set;}
    }
}
