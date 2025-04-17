using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.SubjectServices
{
    public interface ISubjectService
    {
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
