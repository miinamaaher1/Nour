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
using System.Web.Mvc;
using static NetTopologySuite.Geometries.Utilities.GeometryMapper;
using Stripe;

namespace XCourse.Services.Implementations.SubjectServices
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public async Task<IEnumerable<SelectListItem>> GetDistinctTopicsAsync()
        //{
        //    var topics = await _unitOfWork.Subjects
        //        .GetAll() // this must returns IQueryable<Subject> not IEnumerable
        //        .Select(s => s.Topic)
        //        .Distinct()
        //        .ToListAsync(); // ToListAsync works on IQueryable, not IEnumerable

        //    return topics.Select(t => new SelectListItem
        //    {
        //        Value = t,
        //        Text = t
        //    });
        //}
        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            //return await _subjectRepository.GetAllSubjectsAsync();
            return await _unitOfWork.Subjects.GetAllAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            return await _unitOfWork.Subjects
                .FindAsync(s => s.ID == id, new[] { "Teachers" });
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _unitOfWork.Subjects.AddAsync(subject);
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _unitOfWork.Subjects.Update(subject);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = _unitOfWork.Subjects.Get(id);
            if (subject != null)
            {
                _unitOfWork.Subjects.Delete(subject);
                await _unitOfWork.SaveAsync();
            }
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
            return await _unitOfWork.Subjects.FindAsync(s =>
                s.Topic == topic &&
                s.Major == major &&
                s.Year == year &&
                s.Semester == semester
                );
        }

        public async Task AddSubjectAsync(Subject subject, int teacherId)
        {
            var existingSubject = await _unitOfWork.Subjects.FindAsync(s =>
                s.Topic == subject.Topic &&
                s.Major == subject.Major &&
                s.Year == subject.Year &&
                s.Semester ==subject.Semester);

            if (existingSubject == null)
                await _unitOfWork.Subjects.AddAsync(subject);

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
