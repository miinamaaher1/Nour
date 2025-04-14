using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.StudentsViewModels
{
    public class TeacherCardVM
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string TagLine { get; set; }
        public bool IsAvailableForPrivateGroups { get; set; }
        public decimal? PrivateGroupPrice { get; set; }
        public byte[]? TeacherProfilePicture { get; set; }

    }
}
