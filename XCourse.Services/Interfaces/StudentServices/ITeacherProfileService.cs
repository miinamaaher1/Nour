using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface ITeacherProfileService
    {
        TeacherProfileVM CompileTeacherProfile(int teacherID);
    }
}
