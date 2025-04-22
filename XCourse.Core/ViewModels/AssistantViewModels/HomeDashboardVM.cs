using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.AssistantViewModels
{
    public enum SessionStatus
    {
        upcoming,
        ongoing,
        ended
    }
    public class HomeDashboardVM
    {
        public int? AssistantID { get; set; }
        public string? AssistantName { get; set; } = string.Empty;
        public int? TotalGroups { get; set; }
        public List<PendingRequestVM>? PendingRequests { get; set; } = new List<PendingRequestVM>();
        public List<SessionsToday>? SessionsToday { get; set; } = new List<SessionsToday>();
        public List<GroupPerformance>? GroupPerformances { get; set; } = new List<GroupPerformance>();
    }

    public class SessionsToday
    {
        public int SessioId { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public SessionStatus Status { get; set; }
    }

    public class GroupPerformance
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public double StudentsPercentage { get; set; } 
        public double AttendanceRate { get; set; }
    }
}