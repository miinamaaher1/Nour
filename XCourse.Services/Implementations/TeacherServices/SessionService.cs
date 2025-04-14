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
    public class SessionService: ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TeacherSessionVM>> GetSessionsPerTeacher(string guid, int? groupId, int? take = null, int? skip = null)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUserID == guid);
            if (teacher == null) return [];

            if (groupId != null)
            {
                var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId && g.TeacherID == teacher.ID);
                if (group == null) return [];
            }

            var sessions = await _unitOfWork.Sessions.FindAllAsync(
                s => s.Group.TeacherID == teacher.ID && (groupId == null || s.GroupID == groupId),
                includes: ["RoomReservation.Room.Center", "RoomReservation.Room", "Group.Subject"],
                take: take,
                skip: skip
            );

            return sessions.Select(MapSessionToVM).ToList();
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
                CenterName = session.RoomReservation?.Room?.Center?.Name ,
                RoomName = session.RoomReservation?.Room?.Name  ,
                Subject = session.Group?.Subject?.Topic  ,
                Year = (Year)(session.Group?.Subject?.Year),
                Semester = (Semester)(session.Group?.Subject?.Semester) ,
            };
        }
    }
}
