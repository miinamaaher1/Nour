using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class EnrollStudentService : IEnrollStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EnrollStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Enroll(int studentID, int groupID)
        {
            try
            {
                var group = _unitOfWork.Groups.Find(g => g.ID == groupID, ["Students"]);
                var student = _unitOfWork.Students.Get(studentID);
                group.Students.Add(student);
                group.CurrentStudents++;
                if (group.CurrentStudents > group.MaxStudents) throw new Exception();
                _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
