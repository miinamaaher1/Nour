using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface IAttendanceService
    {
        public Task<SessionAttendanceDTO> GetSessionAttendance(SessionAttendanceDTO request);
        public Task<SubmitSessionAttendanceResponse> SubmitSessionAttendance(SessionAttendanceDTO request);

    }
}
