using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Services.Implementations.TeacherServices
{

    //// This service was written by AR12.
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Subject>> GetSubjectsNotTaughtByTeacherAsync(int teacherId, int? take, int? skip)
        {
            IEnumerable<Subject> subjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Teachers.All(t => t.ID != teacherId) , includes: null , skip: skip , take: take);
            
            return subjects ?? Enumerable.Empty<Subject>();
        }

        public async Task<IEnumerable<Subject>> GetSubjectsPerTeacher(int teacherId, int? take, int? skip)
        {

            IEnumerable<Subject> subjects = await _unitOfWork.Subjects.FindAllAsync(sub => sub.Teachers.Any(t=>t.ID ==teacherId), includes: null, skip: skip, take: take);

            return subjects ?? Enumerable.Empty<Subject>();
        }

        public async Task<ResponseSubjectDto>  AssignSubject(int subjectId,int teacherId)
        {
            var subjectResponse = new ResponseSubjectDto
            {
                IsValid = true,
                ErrorMessages = new List<string>()
            };

            var subject = await _unitOfWork.Subjects.FindAsync(s => s.ID == subjectId);
            if (subject == null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Subject not found");
                return subjectResponse;
            }

            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == teacherId);
            teacher.Subjects = new List<Subject>();

            if (teacher == null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Teacher not found");
                return subjectResponse;
            }
            var teacherSubject = await _unitOfWork.Teachers.FindAsync(t => t.ID == teacherId && t.Subjects.Any(s => s.ID == subjectId));
            if (teacherSubject != null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Teacher already has this subject");
                return subjectResponse;
            }


            teacher.Subjects.Add(subject);
            await _unitOfWork.SaveAsync();

            return subjectResponse;
        }


        public async Task<ResponseSubjectDto> RemoveSubject(int subjectId ,int teacherId)
        {
            var subjectResponse = new ResponseSubjectDto
            {
                IsValid = true,
                ErrorMessages = new List<string>()
            };

            var subject =  await _unitOfWork.Subjects.FindAsync(s => s.ID == subjectId);
            if (subject == null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Subject not found");
                return subjectResponse;
            }
            
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == teacherId , ["Subjects"]);

            if (teacher == null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Teacher not found");
                return subjectResponse;
            }

            var teacherSubject = await _unitOfWork.Teachers.FindAsync(t => t.ID == teacherId && t.Subjects.Any(s => s.ID == subjectId));
            if (teacherSubject == null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Teacher doesn't Teach this subject");
                return subjectResponse;
            }


            var isActiveGroup =  await _unitOfWork.Groups.FindAsync(g => g.SubjectID == subjectId 
                                                               && g.TeacherID == teacherId
                                                               && g.IsActive  == true     );
            if (isActiveGroup != null)
            {
                subjectResponse.IsValid = false;
                subjectResponse.ErrorMessages.Add("Teacher has active groups for this subject");
                return subjectResponse;
            }

            
            teacher.Subjects!.Remove(subject);
            await _unitOfWork.SaveAsync();

            return subjectResponse;








        }
        //------------------------------Yasser
        public async Task<IEnumerable<SelectListItem>> GetDistinctTopicsAsync()
        {
            var topics = await _unitOfWork.Subjects
                .FindAllAsync(s => !string.IsNullOrEmpty(s.Topic)) // Fixed predicate to filter valid topics
                .ConfigureAwait(false);

            var distinctTopics = topics
                .Select(s => s.Topic)
                .Distinct();

            return distinctTopics.Select(t => new SelectListItem
            {
                Value = t,
                Text = t
            });
        }
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
                s.Semester == subject.Semester);

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
