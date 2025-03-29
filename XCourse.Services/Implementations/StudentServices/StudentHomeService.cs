using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;

using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.Student;

namespace XCourse.Services.Implementations.Student
{
    public class StudentHomeService : IStudentHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentHomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeViewModel> IndexService(string guid)
        {

            // Fetch the student with all necessary related data
            var student = await _unitOfWork.Students.FindAsync(
                x => x.AppUserID == guid,
                 ["Groups", "AppUser" ]
            );

            if (student == null)
            {
                return new HomeViewModel();
            }

            // Fetch groups based on the student's Major , Year and Location with related data
            var recommendedGroups = await GetRecommendedGroups(student.ID);

            // Fetch all sessions for the groups the student is part of
            var sessions = await GetStudentSessions(student.ID);
            

            // Fetch all announcements for the groups the student is part of
            var announcements = await GetStudentAnnouncements(student.ID);

            // Get the number of unread announcements
            var numOfAnnouncements = announcements.Count;

            var upcomingSessions = sessions
             .Where(s => s.StartDateTime > DateTime.Now)
             .Select(s => new SessionViewModel
             {
                 SessionID = s.ID,
                 IsOnline = s.Group.IsOnline,
                 Duration = s.Duration,
                 StartDateTime = s.StartDateTime,
                 SubjectName = s.Group.Subject != null ? s.Group.Subject.Topic : "Unknown",
                 GroupTeacherName = s.Group.Teacher != null && s.Group.Teacher.AppUser != null
                    ? s.Group.Teacher.AppUser.FirstName + " " + s.Group.Teacher.AppUser.LastName
                    : "Unknown",
                 TeacherProfilePicture =s.Group.Teacher.AppUser != null? s.Group.Teacher.AppUser.ProfilePicture: null
             }).Take(10).ToList();

            var groupViewModels = recommendedGroups
            .Select(g => new GroupViewModel
            {
                GroupID = g.ID,
                GroupTeacherName = g.Teacher != null && g.Teacher.AppUser != null
                    ? g.Teacher.AppUser.FirstName + " " + g.Teacher.AppUser.LastName
                    : "Unknown Group Teacher",
                SubjectName = g.Subject != null ? g.Subject.Topic : "Unknown Subject",
                PricePerSession = g.PricePerSession,
                IsOnline = g.IsOnline,
                IsGirlsOnly = g.IsGirlsOnly,
                MaxStudents = g.MaxStudents,
                CurrentStudentsCount = g.Students != null ? g.Students.Count() : 0,
                TeacherProfilePicture =g.Teacher.AppUser != null ? g.Teacher.AppUser.ProfilePicture : null,
                GroupPicture = g.CoverPicture
            }).Take(10).ToList();

            return new HomeViewModel()
            {
                UpcomingSessions = upcomingSessions,
                RecommendedGroups = groupViewModels,
                Announcements = announcements.Take(10).ToList(),
                NumOfAnnouncements = numOfAnnouncements,

            };

        }


        public async Task<List<Session>> GetStudentSessions(int studentId)
        {

            var sessions = await _unitOfWork.Sessions.FindAllAsync(
                         s => s.Group.Students.Any(st => st.ID == studentId),
                           ["Group", "Group.Subject", "Group.Teacher.AppUser" ]
                     );

            return await Task.FromResult(sessions.OrderBy(s => s.StartDateTime).ToList());

        }


        public async Task<List<Group>> GetStudentGroups(int studentId)
        {
            var groups = _unitOfWork.Groups.FindAll(
                g => g.Students.Any(st => st.ID == studentId),
                new string[] { "Subject", "Students", "Teacher.AppUser" }
            ).ToList();
            return await Task.FromResult(groups);
        }

        public async Task<List<Group>> GetRecommendedGroups (int studentId)
        {
            var student = await _unitOfWork.Students.FindAsync(
                x=>x.ID == studentId,
                    ["Groups", "AppUser" ]
            );

            if (student == null)
            {
                return new List<Group>();
            }

            // Fetch groups based on the student's Major and Year with related data
            var groups = await _unitOfWork.Groups.FindAllAsync(
                   g => g.Subject != null &&
                     g.Subject.Major == student.Major &&
                     g.Subject.Year == student.Year &&
                     g.IsPrivate == false &&
                     g.MaxStudents > g.Students.Count &&
                     !(g.IsGirlsOnly == true && student.AppUser.Gender == Gender.Male) &&
                     g.Address.City.ToLower() == student.AppUser.HomeAddress.City.ToLower() &&
                     !g.Students.Any(st => st.ID == student.ID),
                        ["Subject", "Students", "Teacher.AppUser"]
            );

            return await Task.FromResult(groups.ToList());


        }

        public async Task<List<Announcement>> GetStudentAnnouncements(int studentId)
        {
            var announcements = await _unitOfWork.Announcements.FindAllAsync(
                a => a.Group.Students.Any(st => st.ID == studentId),
                new string[] { "Group", "Group.Teacher.AppUser" }
            );

            return await Task.FromResult(announcements.ToList());
        }

    }
}
