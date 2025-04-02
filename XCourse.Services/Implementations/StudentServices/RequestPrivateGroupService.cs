using XCourse.Core.DTOs;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class RequestPrivateGroupService : IRequestPrivateGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestPrivateGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<PrivateGroupSubjectDTO> PrepareRequest(int studentID, int teacherID)
        {
            List<PrivateGroupSubjectDTO> subjectList;

            var teacherSubjects = _unitOfWork.Teachers.Find(t => t.ID == teacherID, ["Subjects"]).Subjects;
            var student = _unitOfWork.Students.Get(studentID);
            var studentSubjects = _unitOfWork.Subjects.FindAll(s => s.Major == student.Major && s.Year == student.Year);
            var commonSubjects = teacherSubjects.Intersect(studentSubjects).ToList();

            subjectList = commonSubjects != null ? commonSubjects.Select(item => new PrivateGroupSubjectDTO() { Value = item.ID, Display = item.Topic}).ToList() : new();

            return subjectList;
        }

        public RequestStatusDTO SendRequest(PrivateGroupRequestDTO request)
        {
            RequestStatusDTO status = new RequestStatusDTO();
            try
            {
                var _address = new Address()
                {
                    Street = request.Street,
                    Neighborhood = request.Neighborhood,
                    City = request.City,
                    Governorate = request.Governorate
                };

                var _request = new PrivateGroupRequest()
                {
                    StudentID = request.StudentID,
                    TeacherID = request.TeacherID,
                    SubjectID = request.SubjectID,
                    Details = request.Details,
                    Address = _address
                };

                _unitOfWork.PrivateGroupRequests.Add(_request);
                _unitOfWork.Save();

                status.IsValid = true;
                status.Errors = new List<string>();
                return status;
            }
            catch (Exception ex)
            {
                status.IsValid = false;
                status.Errors = new List<string>() { ex.Message};
                return status;
            }
        }
    }
}
