using System.Security.Claims;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface ITeacherProfileService
    {

        Task<ICollection<TeacherCardVM>> GetAllTeachersAsync(ClaimsPrincipal user);
        TeacherProfileVM CompileTeacherProfile(int teacherID, ClaimsPrincipal user);
    }
}
