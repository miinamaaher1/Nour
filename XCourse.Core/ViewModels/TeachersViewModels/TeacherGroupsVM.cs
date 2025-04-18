using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels
{
    public class TeacherGroupsVM
    {
        public int ID { get; set; }

        public byte[]? CoverPicture { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public Address? Address { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsGirlsOnly { get; set; }
        public string? Subject { get; set; }
        public string? CenterName { get; set; }
        public string? RoomName { get; set; } 
        public Major Major { get; set; }
        public Year Year { get; set; }
        public Semester Semester { get; set; }

    }
}
