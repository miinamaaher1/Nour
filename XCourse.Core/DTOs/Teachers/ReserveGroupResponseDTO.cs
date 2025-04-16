using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    public class ReserveGroupResponseDTO
    {
        public bool IsValid { get; set; }
        public List<string>? Errors { get; set; }
        public ReserveGroupBill? Bill { get; set; }
    }
}
