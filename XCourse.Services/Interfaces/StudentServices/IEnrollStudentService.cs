using System.Security.Claims;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface IEnrollStudentService
    {
        Task<DetailedGroupVM> GetGroupInfo(int groupID, ClaimsPrincipal user);
        public Task<bool> Enroll(int studentID, int groupID);
    }
}
