using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using XCourse.Core.DTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;

using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.PaymentService;
using XCourse.Services.Interfaces.Student;

namespace XCourse.Services.Implementations.Student
{
    public class StudentHomeService : IStudentHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITransactionService _transactionService;
        public StudentHomeService(IUnitOfWork unitOfWork, IConfiguration configuration, ITransactionService transactionService )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _transactionService = transactionService;
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

            // Fetch all sessions for the groups the student is part of -->ordered by date  
            var sessions = await GetStudentUpcomingSessions(student.ID);


            // Fetch all announcements for the groups the student is part of --> ordered by date --> 10 announcements 
            var announcements = await GetStudentAnnouncements(student.ID);

            // Get the number of announcements
            var numOfAnnouncements = announcements.Count;

            //Mapping sessions to the Session view model ---> Take 10 sessions  
            var upcomingSessions = await MapSessionsToVMSessions(sessions,10);

            var groupViewModels = recommendedGroups
            .Select(g => new RecommendedGroupViewModel
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
                UpcomingSessions = upcomingSessions.ToList(),
                RecommendedGroups = groupViewModels,
                Announcements = announcements.ToList(),
                NumOfAnnouncements = numOfAnnouncements

            };

        }

        public async Task<ICollection<SessionViewModel>> SessionIndexService(string guid , int groupId)
        {

            if (groupId != 0)
            {
                var sessions = await _unitOfWork.Sessions.FindAllAsync(
                    s => s.GroupID == groupId ,
                    ["Group", "Group.Subject", "Group.Teacher.AppUser"]
                );
                return await MapSessionsToVMSessions(sessions.ToList());
            }

            var student = await _unitOfWork.Students.FindAsync(
                x => x.AppUserID == guid
            );

            var sessionsVM = await MapSessionsToVMSessions(await GetStudentSessions(student.ID));

            return sessionsVM;
        }

        public async Task<SessionDetailsViewModel> SessionDetailsService(int sessionId , string userId)
        {

            var sessionAttendance= await _unitOfWork.Attendances.FindAsync(
                a => a.SessionID == sessionId && a.Student.AppUserID == userId
            );

            var session = await _unitOfWork.Sessions.FindAsync(
                s => s.ID == sessionId,
                ["Group", "Group.Subject","Address", "RoomReservation.Room", "Group.Teacher.AppUser"]
            );
            var wallet = await _unitOfWork.Wallets.FindAsync(w=>w.AppUserID == userId);

            if (session == null)
            {
                return new SessionDetailsViewModel();
            }

            return new SessionDetailsViewModel()
            {
                Session = session,
                Attendances=sessionAttendance,
                WalletBalance = wallet != null ? wallet.Balance : 0,
                MapKey = _configuration["GoogleMaps:ApiKey"],

            };
        }

        public async Task<bool> PayForSessionServiceAsync(int sessionId, string userId)
        {
            
            var session = await _unitOfWork.Sessions.FindAsync(
                s => s.ID == sessionId,
                ["Group", "Group.Teacher.AppUser"]
            );

            if (session == null)
                return false;

            
            var student = await _unitOfWork.Students.FindAsync(s => s.AppUserID == userId);
            if (student == null)
                return false;

            
            var attendance = await _unitOfWork.Attendances.FindAsync(
                a => a.SessionID == sessionId && a.StudentID == student.ID
            );

            string teacherUserId = session.Group.Teacher.AppUserID;

            bool transactionSuccessful = await _transactionService.MakeTransactionAsync(
                userId,
                teacherUserId,
                session.Group.PricePerSession,
                TransactionType.Payment
            );

            if (!transactionSuccessful)
                return false;

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    SessionID = sessionId,
                    StudentID = student.ID,
                    HasPaid = true,
                    HasAttended = false
                };
                await _unitOfWork.Attendances.AddAsync(attendance);
            }
            else
            {
                attendance.HasPaid = true;
                _unitOfWork.Attendances.Update(attendance);
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
        public async Task<bool> SessionSaveFeedbackService(FeedBackDTO feedBackDTO, string userId)
        {
            if (feedBackDTO == null)
                return false;

            var student = _unitOfWork.Students.Find(s => s.AppUserID == userId);
            if (student == null)
                return false;

            feedBackDTO.StudentId = student.ID;

            var feedback = _unitOfWork.Attendances.Find(
                a => a.SessionID == feedBackDTO.SessionID && a.StudentID == feedBackDTO.StudentId
            );

            if (feedback == null)
            {
                feedback = new Attendance
                {
                    StudentID = feedBackDTO.StudentId,
                    SessionID = feedBackDTO.SessionID,
                    Rating = feedBackDTO.Rating,
                    Feedback = feedBackDTO.Feedback
                };

                _unitOfWork.Attendances.Add(feedback);
            }
            else
            {
                feedback.Rating = feedBackDTO.Rating;
                feedback.Feedback = feedBackDTO.Feedback;
                _unitOfWork.Attendances.Update(feedback);
            }

            await _unitOfWork.SaveAsync(); // Prefer async version if available
            return true;
        }



        public async Task<bool> SessionRemoveFeedbackService(FeedBackDTO feedBackDTO)
        {
            if (feedBackDTO == null)
                return false;
            var feedback = _unitOfWork.Attendances.Find(
                a => a.SessionID == feedBackDTO.SessionID && a.StudentID == feedBackDTO.StudentId
            );
            if (feedback != null)
            {
                feedback.Feedback = null; 
                feedback.Rating = null;
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;

        }

        /**----------------------------------------------------------------------------------**/
        public async Task<ICollection<Session>> GetStudentUpcomingSessions(int studentId)
        {

            var sessions = await _unitOfWork.Sessions.FindAllAsync(
                         s => s.Group.Students.Any(st => st.ID == studentId)
                            && s.StartDateTime > DateTime.Now,
                           ["Group", "Group.Subject", "Group.Teacher.AppUser" ]
                     );

            return await Task.FromResult(sessions.OrderBy(s => s.StartDateTime).ToList());

        }

        public async Task<ICollection<Session>> GetStudentSessions(int studentId)
        {

            var sessions = await _unitOfWork.Sessions.FindAllAsync(
                         s => s.Group.Students.Any(st => st.ID == studentId),
                           ["Group", "Group.Subject", "Group.Teacher.AppUser"]
                     );

            return await Task.FromResult(sessions.OrderBy(s => s.StartDateTime).ToList());

        }


        public async Task<List<Group>> GetStudentGroups(int studentId)
        {
            var groups = _unitOfWork.Groups.FindAll(
                g => g.Students.Any(st => st.ID == studentId),
                ["Subject", "Students", "Teacher.AppUser" ]
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
                     g.IsActive == true &&
                     g.Subject.Major == student.Major &&
                     g.Subject.Year == student.Year &&
                     g.IsPrivate == false &&
                     g.MaxStudents > g.CurrentStudents &&
                     !(g.IsGirlsOnly == true && student.AppUser.Gender == Gender.Male) &&
                     (g.IsOnline == true || g.Address.City.ToLower() == student.AppUser.HomeAddress.City.ToLower()) &&
                     !g.Students.Any(st => st.ID == student.ID),
                 ["Subject", "Students", "Teacher.AppUser"],
                 skip: 0,
                 take: 10
            );

            return await Task.FromResult(groups.ToList());


        }

        public async Task<List<AnnouncementVM>> GetStudentAnnouncements(int studentId)
        {

            var studentGroups = await _unitOfWork.Groups.FindAllAsync(g => g.Students!.Any(std => std.ID == studentId));
            var groupIds = studentGroups.Select(g => g.ID).ToList();

            var announcements = await _unitOfWork.Announcements.FindAllAsync(announcement =>
                announcement.Groups!.Any(g => groupIds.Contains(g.ID)), ["Groups", "Groups.Subject"],skip: 0, take: 10, order: a => a.DateTime,direction:Order.DESC
            );

            var announcementVMs = new List<AnnouncementVM>();
            foreach (var announcement in announcements)
            {
                
                var relevantGroups = announcement.Groups!
                    .Where(g => groupIds.Contains(g.ID))
                    .Select(g => new AnnouncementGroup
                    {
                        GroupID = g.ID,
                        GroupSubject = g.Subject?.Topic ?? string.Empty
                    })
                    .ToList();

                announcementVMs.Add(new AnnouncementVM
                {
                    ID = announcement.ID,
                    IsImportant = announcement.IsImportant,
                    Title = announcement.Title,
                    Body = announcement.Body,
                    DateTime = announcement.DateTime,
                    GroupSubjectNames = relevantGroups
                });
            }

            return announcementVMs;
        }

        private async Task<ICollection<SessionViewModel>> MapSessionsToVMSessions(ICollection<Session> sessions , int numOfRows=0)
        {
            var sessionsVM = sessions
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
                     TeacherProfilePicture = s.Group.Teacher.AppUser != null ? s.Group.Teacher.AppUser.ProfilePicture : null
                 });

            if (numOfRows > 0)
            {
                return sessionsVM.Take(numOfRows).ToList();
            }
            else
            {
                return sessionsVM.ToList();
            }
        }

      
    }
}
