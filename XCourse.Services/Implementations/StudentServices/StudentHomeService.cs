using Microsoft.AspNetCore.Mvc;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.Students;
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

        public HomeViewModel IndexService(string guid)
        {

            // Fetch the student with all necessary related data
            var student = _unitOfWork.Students.Find(
                x => x.AppUserID == guid,
                new string[] { "Groups", "AppUser" } // Ensure all required navigation properties are loaded
            );

            if (student == null)
            {
                return new HomeViewModel();
            }

            // Fetch groups based on the student's Major and Year with related data
            var groups = _unitOfWork.Groups.FindAll(
                g => g.Subject != null &&
                     g.Subject.Major == student.Major &&
                     g.Subject.Year == student.Year,
                new string[] { "Subject", "Students", "Teacher.AppUser" } // Ensure Teacher's AppUser is loaded
            ).ToList();

            // Fetch all sessions for the groups the student is part of
            var sessions = _unitOfWork.Sessions.FindAll(
                s => s.Group.Students.Any(st => st.ID == student.ID),
                new string[] { "Group", "Group.Subject", "Group.Teacher.AppUser" } // Load necessary properties
            ).ToList();

            // Fetch all announcements for the groups the student is part of
            var announcements = _unitOfWork.Announcements.FindAll(
                a => a.Group.Students.Any(st => st.ID == student.ID),
                new string[] { "Group", "Group.Teacher.AppUser" } // Ensure Group and Teacher are included
            ).ToList();

            // Get the number of unread announcements
            var numOfAnnouncements = announcements.Count;

            var upcomingSessions = sessions
             .Where(s => s.StartDateTime > DateTime.Now)
             .Select(s => new SessionViewModel
             {
                 SessionID = s.ID,
                 IsOnline = s.Group.IsOnline,
                 
                 StartDateTime = s.StartDateTime,
                 SubjectName = s.Group.Subject != null ? s.Group.Subject.Topic : "Unknown",
                 GroupTeacherName = s.Group.Teacher != null && s.Group.Teacher.AppUser != null
                    ? s.Group.Teacher.AppUser.FirstName + " " + s.Group.Teacher.AppUser.LastName
                    : "Unknown"
             })
             .ToList();

            var groupViewModels = groups
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
                CurrentStudentsCount = g.Students != null ? g.Students.Count() : 0
            })
            .ToList();

            return new HomeViewModel()
            {
                UpcomingSessions = upcomingSessions,
                RecommendedGroups = groupViewModels,
                Announcements = announcements,
                NumOfAnnouncements = numOfAnnouncements
            };

        }
    
    
        //public Task<Group> GetStudentGroups(int studentId)
        //{


        //    var studentGroups = _unitOfWork.Groups
        //      .FindAll(g => g.Students.Any(s => s.ID == studentId))
        //      .ToList();

        //}

    }
}
