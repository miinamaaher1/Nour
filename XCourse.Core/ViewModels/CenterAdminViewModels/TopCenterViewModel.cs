using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class TopCenterViewModel
    {
        public int centerID {  get; set; }
        public string CenterName { get; set; } 
        public int TotalBookings { get; set; } 
    }
}
