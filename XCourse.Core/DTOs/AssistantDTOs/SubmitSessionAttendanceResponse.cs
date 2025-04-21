using NetTopologySuite.Operation.Valid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.AssistantDTOs   
{
    public class SubmitSessionAttendanceResponse
    {
        public bool IsValid { get; set; }
        public virtual List<String>? Errors { get; set; }
    }
}
