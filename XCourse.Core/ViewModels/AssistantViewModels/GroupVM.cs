using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.AssistantViewModels
{
    public class GroupVM
    {
        public int GroupID { get; set; }
        public int TeacherID { get; set; }
        //public int SubjectID { get; set; }
        public string? TeacherName { get; set; } = "";

        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public decimal PricePerSession { get; set; }
        public Address? Address { get; set; }
        public Subject? Subject { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string? CenterName { get; set; } = "";

        public byte[]? GroupPicture { get; set; } = Array.Empty<byte>(); // Named Cover Picture in DB
        public byte[]? TeacherPicture { get; set; } = Array.Empty<byte>();
        public WeekDay DefaultSessionDays { get; set; }
    }

}
