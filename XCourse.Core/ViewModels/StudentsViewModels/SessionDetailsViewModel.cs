using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class SessionDetailsViewModel
    {
        public Session Session { get; set; }= new Session();
        public Attendance Attendances { get; set; } = new Attendance();

        public decimal? WalletBalance { get; set; }
        public string MapKey { get; set; }

    }
}
