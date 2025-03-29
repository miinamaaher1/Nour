using XCourse.Core.ViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface ITeacherProfileService
    {
        TeacherProfileVM CompileTeacherProfile(int teacherID);
    }
}
