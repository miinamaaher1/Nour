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
    public class HomeService: IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TeacherHomeVM> HomeIndexService(string userId)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUserID == userId);
            if (teacher == null)
            {
                return new TeacherHomeVM();
            }

            var groups = await _unitOfWork.Groups.FindAllAsync(
                gr => gr.TeacherID == teacher.ID,
                includes: ["Subject", "DefaultRoom", "DefaultRoom.Center"]
            );

            var sessions = await _unitOfWork.Sessions.FindAllAsync(
                s => s.Group.TeacherID == teacher.ID,
                includes: ["Group.Subject", "RoomReservation.Room.Center"]
            );

            var upcomingSessions = sessions
               
                .ToList();

            var groupVMs = groups.Select(g => new TeacherGroupsVM
            {
                ID = g.ID,
                CoverPicture= g.CoverPicture,
                MaxStudents = g.MaxStudents,
                CurrentStudents = g.CurrentStudents,
                Address = g.Address,
                IsPrivate = g.IsPrivate,
                IsOnline = g.IsOnline,
                IsGirlsOnly = g.IsGirlsOnly,
                Subject = g.Subject?.Topic,
                CenterName = g.DefaultRoom?.Center?.Name,
                RoomName = g.DefaultRoom?.Name,
                Major = g.Subject.Major,
                Year = g.Subject.Year,
                Semester = g.Subject.Semester
            }).ToList();

            var sessionVMs = upcomingSessions.Select(s => new TeacherSessionVM
            {
                ID = s.ID,
                Duration = s.Duration,
                Address = s.Address,
                IsOnline = s.IsOnline,
                StartDateTime = s.StartDateTime,
                CenterName = s.RoomReservation?.Room?.Center?.Name ?? "",
                RoomName = s.RoomReservation?.Room?.Name ?? "",
                Subject = s.Group?.Subject?.Topic ?? "",
                Major = s.Group!.Subject!.Major ,
                Year = s.Group.Subject.Year ,
                Semester = s.Group.Subject.Semester 
            }).ToList();

            return new TeacherHomeVM()
            {
                TeacherGroups = groupVMs,
                WeekSessions= sessionVMs,
            };
        }

    }

}
