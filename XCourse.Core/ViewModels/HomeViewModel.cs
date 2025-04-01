using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.Students
{
    public class HomeViewModel
    {

        public List<SessionViewModel> UpcomingSessions { get; set; } = new();
        public List<GroupViewModel> RecommendedGroups { get; set; } = new();
        public List<Announcement> Announcements { get; set; } = new();
        public int NumOfAnnouncements { get; set; }

    }
}
