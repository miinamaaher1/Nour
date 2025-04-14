using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class TeacherHomeVM
    {
        public List<TeacherGroupsVM> TeacherGroups { get; set; } = new List<TeacherGroupsVM>();
        public List<TeacherSessionVM> WeekSessions { get; set; } = new List<TeacherSessionVM>();

    }
}
