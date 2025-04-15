using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.TeacherServices;

namespace XCourse.Services.Implementations.TeacherServices
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
    }
}
