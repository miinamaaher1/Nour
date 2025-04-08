using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class TeacherProfileService : ITeacherProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public TeacherProfileService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public TeacherProfileVM CompileTeacherProfile(int teacherID, ClaimsPrincipal user)
        {

            var auser = _userManager.GetUserAsync(user).Result;
            var stud = _unitOfWork.Students.Find(s => s.AppUserID == auser.Id, ["Groups"]);

            var teacher = _unitOfWork.Teachers.Find(t => t.ID == teacherID, ["AppUser"]);
            if (teacher == null) return null;

            var profile = new TeacherProfileVM()
            {
                TeacherID = teacherID,
                TeacherName = teacher.AppUser.FirstName + " " + teacher.AppUser.LastName,
                IsAvailableForPrivateGroups = teacher.IsAvailableForPrivateGroups,
                PrivateGroupPrice = teacher.PrivatePrice,
                TeacherProfilePicture = teacher.AppUser.ProfilePicture,
                StudentID = stud.ID,
                AvailbleGroups = new()
            };

            var groups = _unitOfWork.Groups.FindAll(g => g.TeacherID == teacherID && g.IsActive && g.CurrentStudents < g.MaxStudents);
            var availableGroups = groups.Except(stud.Groups).ToList();

            foreach (var group in availableGroups)
            {
                CompactGroupVM compactGroup = new()
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
