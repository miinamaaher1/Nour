using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    public class PostAnnouncementRequestDTO
    {
        public int? AnnouncementId { get; set; }
        public int TeacherId { get; set; }
        public  string? AnnouncementBody { get; set; }
        public string? AnnouncementTitle { get; set; }
        public bool? IsImportant { get; set; }
        public int[]? GroupIds { get; set; } 
        public int Take {get; set;}
        public int Skip {get; set;}
    }
}
