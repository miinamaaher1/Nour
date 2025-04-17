using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Repositories.Interfaces
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Teacher> GetTeacherWithSubjectsAsync(int id);
    }
}
