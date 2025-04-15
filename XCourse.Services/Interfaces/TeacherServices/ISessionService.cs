using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface ISessionService
    {
        Task<Teacher> GetTeacherByUserId(string userId);
        public Task<Session> GetSessionDetailsById(int sessionId, int teacherId);
    }
}
