using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.TeachersViewModels;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface ISessionService
    {
        public Task<IEnumerable<TeacherSessionVM>> GetSessionsPerTeacher(string guid, int? groupId, int? take = null, int? skip = null);

    }
}
