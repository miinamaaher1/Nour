using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using System.Drawing;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class GroupDetailsVM
    {
        public int GroupID { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public decimal PricePerSession { get; set; }
        public Address? Address { get; set; }
        public WeekDay DefaultSessionDays { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public int TeacherID { get; set; }
        public byte[]? CoverPicture { get; set; }
        public int SubjectID { get; set; }
        public string CenterName { get; set; } = "";
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
        public virtual ICollection<SessionDeatils>? Sessions { get; set; }
        public virtual ICollection<GroupAttendanceVM>? GroupAttendanceVM { get; set; }
    }
}
