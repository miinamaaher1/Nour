using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs
{
    public class ErrorStatusDTO
    {
       public string? Title { get; set; }
       public string? message { get; set; }
       public int? statusCode { get; set; }
    }
}
