using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.Student
{
    public interface IStudentHomeService
    {
        public Task<HomeViewModel> IndexService(string id);
        
        public Task<ICollection<SessionViewModel>> SessionIndexService(string guid);

        public Task<SessionDetailsViewModel> SessionDetailsService(int sessionId, string userId);

        public Task<ICollection<Session>> GetStudentSessions(int studentId);

        public Task<List<Group>> GetStudentGroups(int studentId);

        public Task<List<Group>> GetRecommendedGroups(int studentId);

        public Task<List<Announcement>> GetStudentAnnouncements(int studentId);
    }
}
