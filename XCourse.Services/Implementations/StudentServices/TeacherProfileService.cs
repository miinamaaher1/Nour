using XCourse.Core.DTOs;
using XCourse.Core.ViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class TeacherProfileService : ITeacherProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TeacherProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TeacherProfileVM CompileTeacherProfile(int teacherID)
        {
            var teacher = _unitOfWork.Teachers.Find(t => t.ID == teacherID, ["AppUser"]);
            if (teacher == null) return null;

            var profile = new TeacherProfileVM()
            {
                TeacherID = teacherID,
                TeacherName = teacher.AppUser.FirstName + " " + teacher.AppUser.LastName,
                TeacherProfilePicture = teacher.AppUser.ProfilePicture,
                AvailbleGroups = new()
            };

            var groups = _unitOfWork.Groups.FindAll(g => g.TeacherID == teacherID && g.IsActive && g.CurrentStudents < g.MaxStudents);
            foreach (var group in groups)
            {
                CompactGroupDTO compactGroup = new()
                {
                    GroupID = group.ID,
                    IsOnline = group.IsOnline,
                    IsGirlsOnly = group.IsGirlsOnly,
                    CurrentStudents = group.CurrentStudents,
                    MaxStudents = group.MaxStudents,
                    CoverPicture = group.CoverPicture,
                    PricePerSession = group.PricePerSession,
                };

                var subject = _unitOfWork.Subjects.Get(group.SubjectID);
                compactGroup.SubjectName = subject.Topic;
                compactGroup.Semester = subject.Semester;
                compactGroup.Year = subject.Year;

                profile.AvailbleGroups.Add(compactGroup);
            }

            return profile;
        }
    }
}
