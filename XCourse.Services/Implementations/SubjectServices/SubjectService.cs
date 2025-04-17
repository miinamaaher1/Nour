using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Services.Interfaces.SubjectServices;
using XCourse.Infrastructure.Repositories.Interfaces;
using Xcourse.Infrastructure.Repositories;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace XCourse.Services.Implementations.SubjectServices
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(ISubjectRepository subjectRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _subjectRepository.GetAllSubjectsAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            return await _subjectRepository.GetSubjectByIdAsync(id);
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _subjectRepository.AddSubjectAsync(subject);
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            await _subjectRepository.UpdateSubjectAsync(subject);
        }

        public async Task DeleteSubjectAsync(int id)
        {
            await _subjectRepository.DeleteSubjectAsync(id);
        }
        public async Task<Teacher> GetTeacherByUserId(string id)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUser.Id == id);
            return teacher;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByTeacherId(int id)
        {
            var Teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == id, ["Subjects"]);
            if (Teacher.Subjects is null)
            {
                return new List<Subject>();
            }
            else
            {
                return Teacher.Subjects;
            }
        }

        public async Task<Subject> GetSubjectByCriteriaAsync(string topic, Major major, Year year, Semester semester)
        {
            return await _subjectRepository.FindAsync(s =>
                s.Topic == topic &&
                s.Major == major &&
                s.Year == year &&
                s.Semester == semester);
        }

        public async Task AddSubjectAsync(Subject subject, int teacherId)
        {
            var existingSubject = await _subjectRepository.FindByCriteriaAsync(
                subject.Topic, subject.Major, subject.Year, subject.Semester);

            if (existingSubject == null)
                await _subjectRepository.AddSubjectAsync(subject);

            var teacher = await _unitOfWork.Teachers.GetAsync(teacherId);
            if (teacher != null)
            {
                teacher.Subjects ??= new List<Subject>();
                teacher.Subjects.Add(subject);
                _unitOfWork.Teachers.Update(teacher);
                await _unitOfWork.SaveAsync();
            }
        }

    }
}
