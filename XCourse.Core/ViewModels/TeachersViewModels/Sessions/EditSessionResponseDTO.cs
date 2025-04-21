using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.ViewModels.TeachersViewModels.Sessions
{
    public class EditSessionResponseDTO
    {
        public bool Status { get; set; }
        public List<string>? Errors { get; set; }

    }
}
