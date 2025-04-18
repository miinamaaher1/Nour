using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class TeacherSessionVM
    {
        public int ID { get; set; }
        public TimeSpan? Duration { get; set; }
        public Address? Address { get; set; }
        public bool IsOnline { get; set; }
        public DateTime StartDateTime { get; set; }
        public string? CenterName { get; set; } = "";
        public string? RoomName { get; set; } = "";
        public string? Subject { get; set; } = "";
        public Major Major { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }

    }
}
