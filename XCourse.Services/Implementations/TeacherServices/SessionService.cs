using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Core.ViewModels.TeachersViewModels.Sessions;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Implementations.VideoServices;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Services.Implementations.TeacherServices
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IYouTubeUploaderService _youTubeUploaderService;

        public SessionService(IUnitOfWork unitOfWork, IYouTubeUploaderService youTubeUploaderService)
        {
            this._unitOfWork = unitOfWork;
            _youTubeUploaderService = youTubeUploaderService;
        }

        public async Task<Teacher> GetTeacherByUserId(string userId)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUser!.Id == userId);
            return teacher;
        }

        public async Task<IEnumerable<TeacherSessionVM>> GetSessionsPerTeacher(string guid, int? groupId, int? take = null, int? skip = null)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUserID == guid);
            if (teacher == null) return new List<TeacherSessionVM>();

            var sessions = await _unitOfWork.Sessions.FindAllAsync(
                s => s.Group.TeacherID == teacher.ID && (groupId == null || s.GroupID == groupId),
                includes: new[] { "RoomReservation.Room.Center", "RoomReservation.Room", "Group.Subject" },
                take: take,
                skip: skip
            );

            return sessions.Select(MapSessionToVM).ToList();
        }

        public async Task<Session> GetSessionDetailsById(int sessionId, int teacherId)
        {
            var tempSession = new Session();
            if (sessionId <= 0 || teacherId <= 0)
            {
                return tempSession;
            }

            var session = await _unitOfWork.Sessions.FindAsync(session => session.ID == sessionId, includes: new[] { "Group.GroupDefaults" });

            if (session?.Group?.TeacherID != teacherId)
            {
                return tempSession;
            }

            var subject = await _unitOfWork.Subjects.FindAsync(s => s.ID == session.Group.SubjectID);
            session.Group.Subject = subject;

            return session;
        }

        private TeacherSessionVM MapSessionToVM(Session session)
        {
            return new TeacherSessionVM
            {
                ID = session.ID,
                Duration = session.Duration,
                Address = session.Address,
                IsOnline = session.IsOnline,
                StartDateTime = session.StartDateTime,
                CenterName = session.RoomReservation?.Room?.Center?.Name,
                RoomName = session.RoomReservation?.Room?.Name,
                Subject = session.Group?.Subject?.Topic,
                Year = (Year)(session.Group?.Subject?.Year ?? 0),
                Semester = (Semester)(session.Group?.Subject?.Semester ?? 0),
            };
        }

        async public Task<int> GetGroupTypeFromSession(int sessionId, int teacherId)
        {
            // Return type to be converted to an enum LATER
            var session = await _unitOfWork.Sessions.FindAsync(s => s.ID == sessionId, ["Group.GroupDefaults"]);
            var group = session.Group!;
            if (group.TeacherID != teacherId)
            {
                return 0;
            }
            if (group == null)
            {
                return 0; // Invalid Session 
            }
            if (group.IsOnline == true)
            {
                return 1; // Online Group
            }
            if (group.GroupDefaults == null || group.GroupDefaults.Count
                () == 0 || group.GroupDefaults!.Any(gd => gd.Room == null))
            {
                return 2; // Local Group at teacher's Home
            }
            return 3;  // Group Ina center 
        }

        async public Task<ICollection<Session>> GetSessionsInGroup(int groupId, int teacherId)
        {
            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId);
            if (group.TeacherID != teacherId)
            {
                return new List<Session>();
            }
            var sessions = await _unitOfWork.Sessions.FindAllAsync(s => s.GroupID == groupId, ["Address"]);
            return sessions.ToList();
        }
        async public Task<EditSessionResponseDTO> EditOfflineLocalSession(EditOfflineLocalSessionVM sessionVM, int teacherId)
        {
            var errors = new List<string>();

            // Validate inputs
            if (teacherId == 0)
                errors.Add("Invalid teacher ID.");

            if (sessionVM == null)
                errors.Add("Session data is missing.");

            if (sessionVM!.StartTime >= sessionVM.EndTime)
                errors.Add("Start time must be before end time.");

            if ((sessionVM.EndTime - sessionVM.StartTime).TotalMinutes < 30)
                errors.Add("Session must be at least 30 minutes long.");
            if (sessionVM.Address == null)
                errors.Add("Session address is missing.");

            if (sessionVM.Address?.City == null || sessionVM.Address?.Governorate == null || sessionVM.Address?.Street == null || sessionVM.Address?.Neighborhood == null)
                errors.Add("Session address is not complete.");

            if (errors.Any())
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = errors
                };
            }

            // Fetch session with Group
            var session = await _unitOfWork.Sessions.FindAsync(s => s.ID == sessionVM.SessionID, ["Group"]);

            if (session == null)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "Session not found." }
                };
            }

            if (session.Group?.TeacherID != teacherId)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "Invalid request! You don't have access to this action." }
                };
            }

            // Construct full DateTime
            var startDateTime = new DateTime(
            sessionVM.Date.Year,
            sessionVM.Date.Month,
            sessionVM.Date.Day,
            sessionVM.StartTime.Hour,
            sessionVM.StartTime.Minute,
            sessionVM.StartTime.Second);

            var endDateTime = new DateTime(
            sessionVM.Date.Year,
            sessionVM.Date.Month,
            sessionVM.Date.Day,
            sessionVM.EndTime.Hour,
            sessionVM.EndTime.Minute,
            sessionVM.EndTime.Second);

            session.StartDateTime = startDateTime;
            session.EndDateTime = endDateTime;
            session.Duration = endDateTime - startDateTime;

            // Update session date-time fields
            session.StartDateTime = startDateTime;
            session.EndDateTime = endDateTime;
            session.Description = sessionVM.Description;

            //update session location
            session.Location = sessionVM.Location;

            // update session Address
            session.Address = new Address();
            session.Address.Governorate = sessionVM.Address!.Governorate;
            session.Address.City = sessionVM.Address.City;
            session.Address.Neighborhood = sessionVM.Address.Neighborhood;
            session.Address.Street = sessionVM.Address!.Street;

            // Save changes
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "An error occurred while saving the session.", ex.Message }
                };
            }

            return new EditSessionResponseDTO
            {
                Status = true,
                Errors = new List<string>()
            };
        }










        // Edit Session [Based on types utils]
        async public Task<EditSessionResponseDTO> EditOnlineSessionVM(EditOnlineSessionVM sessionVM, int teacherId)
        {
            var errors = new List<string>();

            // Validate inputs
            if (teacherId == 0)
                errors.Add("Invalid teacher ID.");

            if (sessionVM == null)
                errors.Add("Session data is missing.");

            if (sessionVM.StartTime >= sessionVM.EndTime)
                errors.Add("Start time must be before end time.");

            if ((sessionVM.EndTime - sessionVM.StartTime).TotalMinutes < 30)
                errors.Add("Session must be at least 30 minutes long.");

            if (errors.Any())
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = errors
                };
            }

            // Fetch session with Group
            var session = await _unitOfWork.Sessions.FindAsync(s => s.ID == sessionVM.SessionID, ["Group"]);

            if (session == null)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "Session not found." }
                };
            }

            if (session.Group?.TeacherID != teacherId)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "Invalid request! You don't have access to this action." }
                };
            }

            // Construct full DateTime
            var startDateTime = new DateTime(
            sessionVM.Date.Year,
            sessionVM.Date.Month,
            sessionVM.Date.Day,
            sessionVM.StartTime.Hour,
            sessionVM.StartTime.Minute,
            sessionVM.StartTime.Second);

            var endDateTime = new DateTime(
            sessionVM.Date.Year,
            sessionVM.Date.Month,
            sessionVM.Date.Day,
            sessionVM.EndTime.Hour,
            sessionVM.EndTime.Minute,
            sessionVM.EndTime.Second);

            session.StartDateTime = startDateTime;
            session.EndDateTime = endDateTime;
            session.Duration = endDateTime - startDateTime;

            // Update session fields
            session.StartDateTime = startDateTime;
            session.EndDateTime = endDateTime;
            session.Description = sessionVM.Description;

            var thisSession = _unitOfWork.Sessions.Find(s => s.ID == sessionVM.SessionID, ["Group.Teacher.AppUser", "Group.Subject"]);
            var title = thisSession.Group.Teacher.AppUser.FirstName
                        + " " + thisSession.Group.Teacher.AppUser.LastName
                        + " " + thisSession.Group.Subject.Topic
                        + " " + Guid.NewGuid();

            var url = await _youTubeUploaderService.UploadVideoAsync(sessionVM.Video.OpenReadStream(), title, sessionVM.Description);
            
            if (url == null)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "Couldn't upload video." }
                };
            }

            session.URL = url;

            // Save changes
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return new EditSessionResponseDTO
                {
                    Status = false,
                    Errors = new List<string> { "An error occurred while saving the session.", ex.Message }
                };
            }

            return new EditSessionResponseDTO
            {
                Status = true,
                Errors = new List<string>()
            };
        }
    }
}
