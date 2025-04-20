using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.TeachersViewModels.Sessions
{
    public class EditOnlineSessionVM
    {
        public int SessionID { get; set; }
        public IFormFile? Video { get; set; }
        public DateOnly Date {get; set;}
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Description { get; set; }
    }
}
