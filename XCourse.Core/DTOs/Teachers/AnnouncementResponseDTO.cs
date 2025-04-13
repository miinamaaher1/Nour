using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    public class AnnouncementResponseDTO
    {
        public int Id { get; set; }
        public string? AnnouncementBody { get; set; }
        public string? AnnouncementTitle { get; set; }
        public DateTime? DateTime { get; set; }
        public ICollection<AnnouncementGroupDTO>? Groups {get; set;}
    }
    

}
