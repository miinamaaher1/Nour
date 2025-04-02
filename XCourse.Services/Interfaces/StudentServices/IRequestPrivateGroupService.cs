using XCourse.Core.DTOs;
using XCourse.Core.DTOs.StudentDTOs;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface IRequestPrivateGroupService
    {
        List<PrivateGroupSubjectDTO> PrepareRequest(int studentID, int teacherID);
        RequestStatusDTO SendRequest(PrivateGroupRequestDTO request);
    }
}
