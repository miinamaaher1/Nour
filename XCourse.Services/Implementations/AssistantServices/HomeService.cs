using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.AssistantViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Services.Implementations.AssistantServices
{
    public class HomeService:IHomeService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public HomeService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeDashboardVM> DashboardService(ClaimsPrincipal user)
        {
            var userID = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var assistant = await _unitOfWork.Assistants.FindAsync(
                a => a.AppUserID == userID,
                includes: new[] { "AppUser" }
            );

            var invitations = await _unitOfWork.AssistantInvitations.FindAllAsync(i => i.AssistantID == assistant.ID && i.Status == AssistantInvitationStatus.Accepted);

            var groupIds = invitations.Select(i => i.GroupID).ToList();

            if (assistant == null)
                return new HomeDashboardVM();
            var pendingRequests = await GetPendingRequestsAsync(assistant.ID);
            var sessionsToday = await GetSessionsTodayAsync(assistant.ID);
            var groupPerformances = await GetGroupPerformancesAsync(assistant.ID);
            return new HomeDashboardVM
            {
                AssistantID = assistant.ID,
                AssistantName = assistant.AppUser.FirstName + " " + assistant.AppUser.LastName,
                TotalGroups = groupIds.Count,
                PendingRequests = pendingRequests.ToList(),
                SessionsToday = sessionsToday.ToList(),
                GroupPerformances = groupPerformances.ToList()
            };

        }


        private async Task<ICollection<SessionsToday>> GetSessionsTodayAsync(int assistantId)
        {
            var invitations = await _unitOfWork.AssistantInvitations.FindAllAsync(i => i.AssistantID == assistantId && i.Status == AssistantInvitationStatus.Accepted);

            var groupIds = invitations.Select(i => i.GroupID).ToList();

            var sessions = await _unitOfWork.Sessions.FindAllAsync(s => groupIds.Contains(s.GroupID) && s.StartDateTime.Date == DateTime.Now.Date, includes: new[] { "Group", "Group.Teacher.AppUser" });

            var sessionsToday = sessions.Select(s => new SessionsToday
            {
                SessioId = s.ID,
                TeacherName = s.Group.Teacher.AppUser.FirstName + " " + s.Group.Teacher.AppUser.LastName,
                SubjectName = s.Group.Subject.Topic,
                StartTime = s.StartDateTime,
                Duration = (TimeSpan)s.Duration,
                Status = (s.StartDateTime > DateTime.Now) ? SessionStatus.upcoming : (s.EndDateTime < DateTime.Now) ? SessionStatus.ended : SessionStatus.ongoing
            }).ToList();
            return sessionsToday;
        }

        private async Task<ICollection<GroupPerformance>> GetGroupPerformancesAsync(int assistantId)
        {
            var invitations = await _unitOfWork.AssistantInvitations.FindAllAsync(i => i.AssistantID == assistantId && i.Status == AssistantInvitationStatus.Accepted);

            var groupIds = invitations.Select(i => i.GroupID).ToList();
            var groups = await _unitOfWork.Groups.FindAllAsync(g => groupIds.Contains(g.ID), includes: new[] { "Subject", "Teacher.AppUser" });

            var groupPerformanceTasks = groups.Select(async g => new GroupPerformance
            {
                GroupID = g.ID,
                GroupName = g.Subject.Topic,
                TeacherName = g.Teacher.AppUser.FirstName + " " + g.Teacher.AppUser.LastName,
                StudentsPercentage = g.CurrentStudents > 0 ? (double)(g.CurrentStudents / g.MaxStudents) * 100 : 0,
                AttendanceRate = await getAttendanceRate(g.ID),
            });

            var groupPerformances = await Task.WhenAll(groupPerformanceTasks);

            return groupPerformances.ToList();
        }

        private async Task<double> getAttendanceRate(int groupId)
        {
            var attendances = await _unitOfWork.Attendances.FindAllAsync(a => a.Session.GroupID == groupId
                                                                            &&a.Session.StartDateTime<DateTime.Now
                                                                            &&a.HasAttended==true);
            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId);

            if (attendances.Count() == 0)
                return 0;
            var totalSessions = attendances.Count();
            var totalAttendedStudents = attendances.Count();
            var totalStudents = group.CurrentStudents * totalSessions;
            return (double)(totalAttendedStudents / totalStudents) * 100;
        }

        private async Task<ICollection<PendingRequestVM>> GetPendingRequestsAsync(int assistantId)
        {
            var assistant = await _unitOfWork.Assistants.FindAsync(a => a.ID == assistantId);

            if (assistant == null)
                return new List<PendingRequestVM>();

            var pendingRequests = await _unitOfWork.AssistantInvitations
                .FindAllAsync(
                    x => x.AssistantID == assistant.ID && x.Status == AssistantInvitationStatus.Pending,
                    includes: new[] { "Group.Teacher.AppUser", "Group.Teacher.Subjects" },
                    skip: 0,
                    take: 3
                );

            return pendingRequests.Select(x => new PendingRequestVM
            {
                Status = x.Status,
                InvitationID = x.ID,
                GroupID = x.GroupID,
                TeacherID = x.Group.TeacherID,
                GroupDescription = x.Group.Description,
                TeacherName = x.Group.Teacher.AppUser.FirstName + " " + x.Group.Teacher.AppUser.LastName,
                SubjectName = x.Group.Subject.Topic,

                Major = x.Group.Subject.Major,
                Year = x.Group.Subject.Year,

                Semester = x.Group.Subject.Semester,

            }).ToList();
        }
    }
}
