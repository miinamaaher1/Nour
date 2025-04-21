using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class RoomHomeViewModel
    {
        
            public int CenterId { get; set; } 
            public string CenterName { get; set; } 
            public int TotalRooms { get; set; } 
            public int BookedRooms { get; set; } 
            public int AvailableRooms => TotalRooms - BookedRooms; 
        
    }
}
