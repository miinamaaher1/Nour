using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    public class PostAnnouncementRequestDTO
    {
        public  string? AnnouncementBody { get; set; }
        public string? AnnouncementTitle { get; set; }
        public int[]? GroupsIds { get; set; }
    }
}
