using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs.Teachers
{
    public class InviteAssistantDTO
    {
        public int TeacherId { get; set; }
        public int AssistantId { get; set; }
        public int GroupId {get; set;}
    }
}
