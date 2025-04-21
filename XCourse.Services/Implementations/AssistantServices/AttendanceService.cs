using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.AssistantDTOs;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.AssistantServices;

namespace XCourse.Services.Implementations.AssistantServices
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Assistant> GetAssistantByUserId(string userId)
        {
            var assistant = await _unitOfWork.Assistants.FindAsync(t => t.AppUser!.Id == userId);
            return assistant;
        }

        public async Task<SessionAttendanceDTO> GetSessionAttendance(SessionAttendanceDTO request)
        {
            var response = new SessionAttendanceDTO
            {
                Errors = new List<string>(),
                IsValid = true
            };

            if (request.SessionId <= 0 || request.AssistantId <= 0)
            {
                response.IsValid = false;
                response.Errors.Add("Invalid Request, Something went wrong");
                return response;
            }

            var session = await _unitOfWork.Sessions.FindAsync(
                s => s.ID == request.SessionId,
                includes: new[] { "Group" });

            if (session == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invalid session, it might have been deleted or you don't have access");
                return response;
            }
            
            var invitation = await _unitOfWork.AssistantInvitations.FindAsync(inv => inv.GroupID == session!.Group!.ID &&  inv.AssistantID == request.AssistantId);
            if (invitation == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invalid Request, You don't have access to this session");
                return response;
            }

            var group = await _unitOfWork.Groups.FindAsync(
                g => g.ID == session.GroupID,
                includes: new[] { "Students.AppUser" });

            var students = group.Students;

            response.SessionId = request.SessionId;
            response.AssistantId = request.AssistantId;
            response.SessionDayAttendances = new List<SessionDayAttendance>();

            if (!students!.Any())
                return response;

            var studentIds = students!.Select(s => s.ID).ToList();

            var attendances = await _unitOfWork.Attendances.FindAllAsync(
                att => studentIds.Contains(att.StudentID) && att.SessionID == request.SessionId);

            foreach (var student in students!)
            {
                var attendance = attendances.FirstOrDefault(att => att.StudentID == student.ID);
                var appUser = student.AppUser;

                response.SessionDayAttendances.Add(new SessionDayAttendance
                {
                    SessionId = request.SessionId,
                    StudentId = student.ID,
                    StudentName = $"{appUser?.FirstName} {appUser?.LastName}",
                    HasAttended = attendance?.HasAttended ?? false,
                    HasPaid = attendance?.HasPaid ?? false,
                    ClassWorkGrade = attendance?.ClassWorkGrade,
                    HomeWorkGrade = attendance?.HomeWorkGrade
                });
            }

            return response;
        }
        public async Task<SubmitSessionAttendanceResponse> SubmitSessionAttendance(SessionAttendanceDTO request)
        {
            var response = new SubmitSessionAttendanceResponse
            {
                Errors = new List<string>(),
                IsValid = true
            };

            if (request.SessionId <= 0 || request.AssistantId <= 0)
            {
                response.IsValid = false;
                response.Errors.Add("Invalid request. Session or Teacher ID is missing.");
                return response;
            }

            var session = await _unitOfWork.Sessions.FindAsync(
                s => s.ID == request.SessionId,
                includes: new[] { "Group" }
            );

            var invitation = await _unitOfWork.AssistantInvitations.FindAsync(inv => inv.GroupID == session!.Group!.ID && inv.AssistantID == request.AssistantId && inv.Status == AssistantInvitationStatus.Accepted);
            if (invitation == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invalid Request, You don't have access to this session");
                return response;
            }

            var sessionAttendances = await _unitOfWork.Attendances
                .FindAllAsync(att => att.SessionID == request.SessionId);

            var attendanceStudentIds = sessionAttendances.Select(a => a.StudentID).ToHashSet();

            foreach (var newAttendance in request.SessionDayAttendances ?? new List<SessionDayAttendance>())
            {
                if (attendanceStudentIds.Contains(newAttendance.StudentId))
                {
                    var existing = sessionAttendances.First(a => a.StudentID == newAttendance.StudentId);
                    existing.HasPaid = newAttendance.HasPaid;
                    existing.HasAttended = newAttendance.HasAttended;
                    existing.HomeWorkGrade = newAttendance.HomeWorkGrade;
                    existing.ClassWorkGrade = newAttendance.ClassWorkGrade;
                }
                else
                {
                    var attendance = new Attendance
                    {
                        SessionID = request.SessionId,
                        StudentID = newAttendance.StudentId,
                        HasPaid = newAttendance.HasPaid,
                        HasAttended = newAttendance.HasAttended,
                        HomeWorkGrade = newAttendance.HomeWorkGrade,
                        ClassWorkGrade = newAttendance.ClassWorkGrade
                    };

                    _unitOfWork.Attendances.Add(attendance);
                }
            }

            try
            {
                int res = await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.Errors.Add("An error occurred while saving attendance.");
            }

            return response;
        }

    }
}
