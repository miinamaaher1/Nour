using Microsoft.AspNetCore.Mvc;
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
    }
}
