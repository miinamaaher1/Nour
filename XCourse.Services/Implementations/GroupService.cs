using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.Teachers;

namespace XCourse.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ResponseCenterDto>> GetAllCentersPerGovernorate(string governorate)
        {
            var centers = await _unitOfWork.Centers.FindAllAsync(c =>
                c.Address != null &&
                c.Address.Governorate != null &&
                c.Address.Governorate.ToLower() == governorate.ToLower());

            return centers.Select(c => new ResponseCenterDto
            {
                CenterId = c.ID,
                CenterName = c.Name
            });
        }
        public async Task<IEnumerable<Room>> GetAllAvailableRooms(RequestRoomDto request)
        {
            var allRoomsInTheCenter = await _unitOfWork.Rooms
                .FindAllAsync(r => r.CenterID == request.CenterID && r.Capacity >= request.Capacity && r.Equipment == Equipment.Lecture);

            var allUnavailableRoomsReservations = await _unitOfWork.RoomReservations
                .FindAllAsync(reservation =>
                    (reservation.WeekDay & request.Day) == request.Day &&
                    reservation.Date >= request.StartDate && reservation.Date <= request.EndDate &&
                    (
                        (request.StartTime >= reservation.StartTime && request.StartTime < reservation.EndTime) ||
                        (request.EndTime > reservation.StartTime && request.EndTime <= reservation.EndTime) ||
                        (request.StartTime <= reservation.StartTime && request.EndTime >= reservation.EndTime)
                    )
                );


            var unavailableRoomIds = allUnavailableRoomsReservations
                .Select(r => r.RoomID)
                .Distinct()
                .ToHashSet();


            var availableRooms = allRoomsInTheCenter
                .Where(room => !unavailableRoomIds.Contains(room.ID))
                .ToList();

            return availableRooms;
        }
        public async Task<IEnumerable<string>> GetAllGovernorates()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Subject>> GetMatchingSubjects(RequestSubjectDto request)
        {
            var subjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Year == request.Year && s.Semester == request.Semester);
            return subjects;
        }
        public async Task<bool> ReserveAnOfflineGroupInCenter(RequestOfflineGroupReservation request)
        {
            if (request.Sessions == null || request.Sessions.Count == 0)
            {
                return false;
            }
            for (int i = 0; i < request.Sessions.Count; i++)
            {
                var reservation = await _unitOfWork.RoomReservations.FindAsync(reservation =>
                    (reservation.WeekDay & request.Sessions[i].DayId) == request.Sessions[i].DayId &&
                    reservation.Date >= request.Sessions[i].StartDate && reservation.Date <= request.Sessions[i].EndDate &&
                    (
                        (request.Sessions[i].StartTime >= reservation.StartTime && request.Sessions[i].StartTime < reservation.EndTime) ||
                        (request.Sessions[i].EndTime > reservation.StartTime && request.Sessions[i].EndTime <= reservation.EndTime) ||
                        (request.Sessions[i].StartTime <= reservation.StartTime && request.Sessions[i].EndTime >= reservation.EndTime)
                    )
                );
                if (reservation == null)
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<IEnumerable<GroupVM>> GetAllGroups(int teacherId)
        {
            var groups = await _unitOfWork.Groups.FindAllAsync(g => g.TeacherID == teacherId && g.IsActive == true, ["GroupDefaults.Room.Center", "Subject"]);
            List<GroupVM> groupVMList = new List<GroupVM>();
            foreach (var group in groups)
            {
                GroupVM groupVM = new GroupVM();

                groupVM.GroupID = group.ID;
                groupVM.Address = group.Address;
                groupVM.IsGirlsOnly = group.IsGirlsOnly;
                groupVM.IsOnline = group.IsOnline;
                groupVM.Capacity = group.MaxStudents;
                groupVM.NumberOfStudent = group.CurrentStudents;
                groupVM.IsPrivate = group.IsPrivate;
                groupVM.Subject = group.Subject;
                if (group.GroupDefaults?.Count() > 0 && group.GroupDefaults.ToList()[0]?.Room?.Center?.Name != null)
                {
                    groupVM.CenterName = group.GroupDefaults.ToList()[0]?.Room?.Center?.Name;
                }
                groupVM.Sessions = new List<SessionDeatils>();
                List<GroupDefaults> groupDefaults = group.GroupDefaults!.ToList();
                for (int i = 0; i < groupDefaults.Count(); i++)
                {
                    SessionDeatils sessionDeatils = new SessionDeatils();
                    sessionDeatils.StartDate = groupDefaults[i].StartDate;
                    sessionDeatils.EndDate = groupDefaults[i].EndDate;
                    sessionDeatils.StartTime = groupDefaults[i].StartTime;
                    sessionDeatils.EndTime = groupDefaults[i].EndTime;
                    sessionDeatils.WeekDay = groupDefaults[i].WeekDay;
                    groupVM.Sessions.Add(sessionDeatils);
                }
                groupVMList.Add(groupVM);
            }

            return groupVMList;
        }
        async public Task<Teacher> GetTeacherByUserId(string userId)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.AppUser!.Id == userId);
            return teacher;
        }
        async public Task<GroupDetailsVM> GetGroupDetailsById(int id)
        {
            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == id, ["Subject", "Teacher", "GroupDefaults.Room.Center", "Students.AppUser"]);
            GroupDetailsVM groupDetailsVM = new();
            // Mapping 
            groupDetailsVM.GroupID = group.ID;

            groupDetailsVM.IsPrivate = group.IsPrivate;
            groupDetailsVM.IsGirlsOnly = group.IsGirlsOnly;
            groupDetailsVM.IsActive = group.IsActive;
            groupDetailsVM.IsOnline = group.IsOnline;

            groupDetailsVM.CoverPicture = group.CoverPicture;

            groupDetailsVM.CurrentStudents = group.CurrentStudents;
            groupDetailsVM.MaxStudents = group.MaxStudents;
            groupDetailsVM.PricePerSession = group.PricePerSession;
            groupDetailsVM.Address = group.Address;
            groupDetailsVM.SubjectID = groupDetailsVM.SubjectID;
            groupDetailsVM.Subject = group.Subject;
            groupDetailsVM.DefaultSessionDays = group.DefaultSessionDays;
            groupDetailsVM.Sessions = new List<SessionDeatils>();

            List<GroupDefaults> groupDefaults = group.GroupDefaults!.ToList();

            if (group.GroupDefaults?.Count() > 0 && group.GroupDefaults.ToList()[0]?.Room?.Center?.Name != null)
            {
                groupDetailsVM.CenterName = group.GroupDefaults.ToList()[0]?.Room?.Center?.Name!;
            }

            for (int i = 0; i < groupDefaults.Count(); i++)
            {
                SessionDeatils sessionDeatils = new SessionDeatils();
                sessionDeatils.StartDate = groupDefaults[i].StartDate;
                sessionDeatils.EndDate = groupDefaults[i].EndDate;
                sessionDeatils.StartTime = groupDefaults[i].StartTime;
                sessionDeatils.EndTime = groupDefaults[i].EndTime;
                sessionDeatils.WeekDay = groupDefaults[i].WeekDay;
                groupDetailsVM.Sessions?.Add(sessionDeatils);
            }
            groupDetailsVM.Students = group.Students;
            return groupDetailsVM;
        }
        public async Task<bool> PostAnnouncement(int groupId, int teacherId, string body, bool isImportant, string? title)
        {
            // Ensure the group exists and belongs to the teacher
            var group = await _unitOfWork.Groups.FindAsync(g => g.ID == groupId && g.TeacherID == teacherId);
            if (group == null)  // Group not found or doesn't belong to the teacher
            {
                return false;
            }

            try
            {
                // Create the announcement
                Announcement announcement = new Announcement
                {
                    Title = string.IsNullOrWhiteSpace(title) ? null : title, // Assign title properly
                    Body = body,
                    DateTime = DateTime.Now,
                    GroupID = groupId
                };

                _unitOfWork.Announcements.Add(announcement);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}