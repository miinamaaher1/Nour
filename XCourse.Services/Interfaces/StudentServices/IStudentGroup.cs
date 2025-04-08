using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface IStudentGroup
    {

        public List<StudentGroup> GetStudentGroup(string id);
        public GroupDetails Details(int id);




    }
}
