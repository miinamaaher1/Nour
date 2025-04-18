using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface ISessionService
    {
        Task<Teacher> GetTeacherByUserId(string userId);
        public Task<Session> GetSessionDetailsById(int sessionId, int teacherId);
        public Task<IEnumerable<TeacherSessionVM>> GetSessionsPerTeacher(string guid, int? groupId, int? take = null, int? skip = null);

    }
}
