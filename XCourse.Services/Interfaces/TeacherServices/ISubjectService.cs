

using Microsoft.AspNetCore.Mvc;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.TeacherServices
{
    public interface ISubjectService
    {
        public Task<IEnumerable<Subject>> GetSubjectsNotTaughtByTeacherAsync(int teacherId, int? take, int? skip);
        Task<IEnumerable<Subject>> GetSubjectsPerTeacher(int teacherId, int? take, int? skip);
        public Task<ResponseSubjectDto> AssignSubject(int subjectId, int teacherId);
        public Task<ResponseSubjectDto> RemoveSubject(int subjectId, int teacherId);
    }
}
