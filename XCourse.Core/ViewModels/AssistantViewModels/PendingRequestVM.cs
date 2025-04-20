using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.AssistantViewModels
{
    public class PendingRequestVM
    {
        public AssistantInvitationStatus Status { get; set; }
        public int InvitationID { get; set; }
        public int GroupID { get; set; }
        public int TeacherID { get; set; }
        public string? TeacherName { get; set; } = "";
        public string? GroupDescription { get; set; } = "";
        public string? SubjectName { get; set; } = "";
        public Major Major { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }

    }
}
