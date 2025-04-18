using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class AnnouncementDataVM
    {
        public int Id { get; set; }
        public string? AnnouncementBody { get; set; }
        public string? AnnouncementTitle { get; set; }
        public DateTime? DateTime { get; set; }
        public bool IsImportant { get; set; }
        public ICollection<AnnouncementGroupDTO>? Groups {get; set;}
    }
    

}
