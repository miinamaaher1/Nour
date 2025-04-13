using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.TeachersViewModels;

namespace XCourse.Core.DTOs.Teachers
{
    public class PostAnnouncementResponseDTO
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<AnnouncementDataVM> Announcements { get; set; } = new List<AnnouncementDataVM>();
    }
}
