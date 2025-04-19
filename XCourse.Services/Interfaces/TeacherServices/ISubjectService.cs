

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

        //------------------------
        public Task<IEnumerable<Subject>> GetAllSubjectsAsync();

        public Task<Subject> GetSubjectByIdAsync(int id);

        public Task AddSubjectAsync(Subject subject);

        public Task UpdateSubjectAsync(Subject subject);

        public Task DeleteSubjectAsync(int id);

        public Task<Teacher> GetTeacherByUserId(string id);

        public Task<IEnumerable<Subject>> GetSubjectsByTeacherId(int id);

        public Task<Subject> GetSubjectByCriteriaAsync(string topic, Major major, Year year, Semester semester);

        public Task AddSubjectAsync(Subject subject, int teacherId);
    }
}
