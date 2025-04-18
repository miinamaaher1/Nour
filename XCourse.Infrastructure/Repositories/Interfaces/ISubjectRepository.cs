using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        public Task<IEnumerable<Subject>> GetAllSubjectsAsync();

        public Task<Subject> GetSubjectByIdAsync(int id);

        public Task AddSubjectAsync(Subject subject);

        public Task UpdateSubjectAsync(Subject subject);

        public Task DeleteSubjectAsync(int id);

        public Task<Subject> FindAsync(Expression<Func<Subject, bool>> predicate);

        public Task<Subject> FindByCriteriaAsync(string topic, Major major, Year year, Semester semester);
    }
}

