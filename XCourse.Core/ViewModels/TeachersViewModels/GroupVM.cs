using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class GroupVM
    {
        public int GroupID { get; set; }
        public bool IsOnline { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsGirlsOnly { get; set; }
        public int NumberOfStudent { get; set; }
        public int Capacity { get; set; }
        public Address? Address { get; set; }
        public Subject? Subject { get; set; }
        public string? CenterName { get; set; } = "";
        public List<SessionDeatils>? Sessions { get; set; }
    }
    public class SessionDeatils
    {
        public WeekDay? WeekDay { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
