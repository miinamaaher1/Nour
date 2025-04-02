using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.Students
{
    public class SessionViewModel
    {
        public int SessionID { get; set; }
        public bool IsOnline { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime StartDateTime { get; set; }
        public string SubjectName { get; set; }
        public string GroupTeacherName { get; set; }
    }
}
