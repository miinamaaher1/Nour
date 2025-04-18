using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories.Interfaces;

namespace XCourse.Infrastructure.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(XCourseContext context) : base(context)
        {
        }

        public async Task<Teacher> GetTeacherWithSubjectsAsync(int id)
        {
            return await context.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.ID == id);
        }
    }
}
