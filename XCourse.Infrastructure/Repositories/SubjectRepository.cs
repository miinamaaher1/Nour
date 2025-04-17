using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories.Interfaces;

namespace XCourse.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly XCourseContext _context;

        public SubjectRepository(XCourseContext context)
        {
            _context = context;
        }

        public async Task<Subject> FindAsync(Expression<Func<Subject, bool>> predicate)
        {
            return await _context.Subjects.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects.Include(s => s.Teachers).FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Subject?> FindByCriteriaAsync(string topic, Major major, Year year, Semester semester)
        {
            return await _context.Subjects.FirstOrDefaultAsync(s =>
                s.Topic == topic &&
                s.Major == major &&
                s.Year == year &&
                s.Semester == semester);
        }
    }
}
