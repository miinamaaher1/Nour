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
                TeacherPhone = teacher.AppUser.PhoneNumber,
                TeacherEmail = teacher.AppUser.Email,
                TagLine = teacher.TagLine,
                IsAvailableForPrivateGroups = teacher.IsAvailableForPrivateGroups,
                PrivateGroupPrice = teacher.PrivatePrice,
                TeacherProfilePicture = teacher.AppUser.ProfilePicture,
                StudentID = stud.ID,
                AvailbleGroups = new()
            };

            var groups = _unitOfWork.Groups.FindAll(g => g.TeacherID == teacherID 
                                                         && g.IsActive
                                                         && g.CurrentStudents < g.MaxStudents
                                                         && g.Subject.Year == stud.Year
                                                         && g.Subject.Major == stud.Major
                                                         , ["Subject"]);

            var availableGroups = groups.Except(stud.Groups).ToList();

            if (auser.Gender == Gender.Male)
            {
                availableGroups = availableGroups.Where(g => g.IsGirlsOnly == false).ToList();
            }

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

        public async Task<ICollection<TeacherCardVM>> GetAllTeachersAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.GetUserAsync(user);

            var student = _unitOfWork.Students.Find(s => s.AppUserID == appUser.Id  );

            if (student == null) return new List<TeacherCardVM>();

            var teachers = _unitOfWork.Teachers.FindAll( t=> t.Subjects!.Any(s=> s.Year== student.Year && s.Major==student.Major  ), ["Subjects", "AppUser"]);

            if (teachers == null) return new List<TeacherCardVM>();

            var teacherCards = teachers.Select(

                t => new TeacherCardVM()
                {
                    TeacherID=t.ID,
                    TeacherName = t.AppUser!.FirstName + " " + t.AppUser.LastName,
                    TeacherProfilePicture = t.AppUser.ProfilePicture,
                    TagLine = t.TagLine!,
                    IsAvailableForPrivateGroups = t.IsAvailableForPrivateGroups,
                    PrivateGroupPrice = t.PrivatePrice,


                }

            ).ToList();

            return Task.FromResult(teacherCards).Result;

        }
    }
}
