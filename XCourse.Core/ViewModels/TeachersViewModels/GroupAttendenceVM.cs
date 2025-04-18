using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class GroupAttendanceVM
    {
        public ICollection<Attendance>  LastFiveAttendencies { get; set; } = new List<Attendance>();
        public string StudentName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public  double? LastClassWorkGrade { get; set; }
        public double? LastHomeWorkGrade { get; set; }
    }
}
