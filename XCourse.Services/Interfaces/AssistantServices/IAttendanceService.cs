using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.AssistantDTOs;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.AssistantServices
{
    public interface IAttendanceService
    {
        public Task<Assistant> GetAssistantByUserId(string userId);
        public Task<SessionAttendanceDTO> GetSessionAttendance(SessionAttendanceDTO request);
        public Task<SubmitSessionAttendanceResponse> SubmitSessionAttendance(SessionAttendanceDTO request);
    }
}
