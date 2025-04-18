using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.SubjectServices;

namespace XCourse.Services.Implementations.SubjectServices
{
    class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository? _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<Teacher> GetTeacherWithSubjectsAsync(int id)
        {
            return await _teacherRepository.GetTeacherWithSubjectsAsync(id);
        }
    }
}
