using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Core.ViewModels.TeachersViewModels.Sessions;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface ISessionService
    {
        public Task<Teacher> GetTeacherByUserId(string userId);
        public Task<Session> GetSessionDetailsById(int sessionId, int teacherId);
        public Task<IEnumerable<TeacherSessionVM>> GetSessionsPerTeacher(string guid, int? groupId, int? take = null, int? skip = null);
        public Task<int> GetGroupTypeFromSession(int sessionId, int TeacherId);
        public Task<ICollection<Session>> GetSessionsInGroup(int groupId, int teacherId);
        public Task<EditSessionResponseDTO> EditOnlineSessionVM(EditOnlineSessionVM sessionVM, int teacherId);
        public Task<EditSessionResponseDTO> EditOfflineLocalSession(EditOfflineLocalSessionVM sessionVM, int teacherId);
    }
}
