using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teachers;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.Teachers;

namespace XCourse.Services.Implementations.TeacherServices
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                        request.StartTime >= reservation.StartTime && request.StartTime < reservation.EndTime ||
                        request.EndTime > reservation.StartTime && request.EndTime <= reservation.EndTime ||
                        request.StartTime <= reservation.StartTime && request.EndTime >= reservation.EndTime
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
            var teacherSubjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Teachers!.Any(t => t.ID == request.TeacherId));

            var filteredSubjects = teacherSubjects
                .Where(s => s.Year == request.Year && s.Semester == request.Semester)
                .ToList();

            return filteredSubjects;

        }
        public async Task<IEnumerable<Subject>> GetAllSubjects(RequestSubjectDto request)
        {
            var teacherSubjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Teachers!.Any(t => t.ID == request.TeacherId));
            return teacherSubjects;

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
                        request.Sessions[i].StartTime >= reservation.StartTime && request.Sessions[i].StartTime < reservation.EndTime ||
                        request.Sessions[i].EndTime > reservation.StartTime && request.Sessions[i].EndTime <= reservation.EndTime ||
                        request.Sessions[i].StartTime <= reservation.StartTime && request.Sessions[i].EndTime >= reservation.EndTime
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
            groupDetailsVM.Students = group.Students;

            var announcements = await _unitOfWork.Attendances.FindAllAsync(a => a.Session.GroupID == id);



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

            // Group Attendance Mapping
            groupDetailsVM.GroupAttendanceVM = new List<GroupAttendanceVM>();

            foreach (var student in group.Students)
            {
                var studentAttendances = await _unitOfWork.Attendances
                    .FindAllAsync(a => a.StudentID == student.ID && a.Session.GroupID == id, ["Session"]);

                var lastFive = studentAttendances
                    .OrderByDescending(a => a.Session!.StartDateTime)
                    .Take(5)
                    .ToList();

                var groupAttendanceVM = new GroupAttendanceVM
                {
                    StudentName = student.AppUser?.FirstName + " " + student.AppUser?.LastName ?? "Unknown",
                    Email = student.AppUser?.Email!,
                    LastFiveAttendencies = lastFive,
                    LastClassWorkGrade = lastFive.FirstOrDefault()?.ClassWorkGrade,
                    LastHomeWorkGrade = lastFive.FirstOrDefault()?.HomeWorkGrade
                };

                groupDetailsVM.GroupAttendanceVM.Add(groupAttendanceVM);
            }


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
                    Groups = new List<Group>()
                };
                announcement.Groups.Add(group);
                return true;

            }
            catch
            {
                return false;
            }
        }

        async public Task<ReserveGroupResponseDTO> ReserveOnlineGroup(ReserveOnlineGroupRequestDTO request)
        {
            if (request.TeacherId <= 0)
            {
                return new ReserveGroupResponseDTO
                {
                    IsValid = false,
                    Errors = ["Something went wrong! Invalid Teacher Id"]
                };
            }
            var response = new ReserveGroupResponseDTO();
            response.IsValid = true;
            response.Errors = new List<string>();
            
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == request.TeacherId, ["Subjects"]);
            var validationResponse = ValidateRequest(request, teacher);

            if (!validationResponse.IsValid)
                return validationResponse;

            // Creating the group 
            Group onlineGroup = new Group();

            // Assigning properties
            onlineGroup.SubjectID = request.SubjectId;
            onlineGroup.Address = null;
            onlineGroup.IsPrivate = request.IsPrivate;
            onlineGroup.IsGirlsOnly = request.IsGirlsOnly;
            onlineGroup.IsActive = true;
            onlineGroup.CurrentStudents = 0;
            onlineGroup.TeacherID = request.TeacherId;
            onlineGroup.PricePerSession = request.PricePerSession;
            onlineGroup.MaxStudents = request.MaxNumberOfStudents;
            

            // Reserving sessions 
            onlineGroup.Sessions = new List<Session>();
            foreach (var defaultSession in request.DefaultSessionResrvations!)
            {
                DateOnly current = request.StartDate;

                while (!MatchesWeekDay(current, defaultSession.WeekDay))
                {
                    current = current.AddDays(1);
                }

                while (current <= request.EndDate)
                {
                    var session = new Session
                    {
                        StartDateTime = current.ToDateTime(defaultSession.StartTime),
                        EndDateTime = current.ToDateTime(defaultSession.EndTime),
                        Duration = defaultSession.EndTime.ToTimeSpan() - defaultSession.StartTime.ToTimeSpan(),
                        Location = null,
                        IsOnline = true,
                        Address = null
                    };

                    onlineGroup.Sessions.Add(session);
                    current = current.AddDays(7);
                }
            }

            try
            {
                await _unitOfWork.Groups.AddAsync(onlineGroup);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return new ReserveGroupResponseDTO()
                {
                    IsValid = false,
                    Errors = ["Something went wrong while saving the data! try again later"]
                };
            }

            return response;
        }



        // Utils methods
        private bool MatchesWeekDay(DateOnly date, WeekDay weekDays)
        {
            var day = date.DayOfWeek switch
            {
                DayOfWeek.Saturday => WeekDay.Saturday,
                DayOfWeek.Sunday => WeekDay.Sunday,
                DayOfWeek.Monday => WeekDay.Monday,
                DayOfWeek.Tuesday => WeekDay.Tuesday,
                DayOfWeek.Wednesday => WeekDay.Wednesday,
                DayOfWeek.Thursday => WeekDay.Thursday,
                DayOfWeek.Friday => WeekDay.Friday,
                _ => WeekDay.None
            };

            return (weekDays & day) != 0;
        }
        private ReserveGroupResponseDTO ValidateRequest(ReserveOnlineGroupRequestDTO request, Teacher? teacher)
        {
            var errors = new List<string>();

            if (request.DefaultSessionResrvations == null ||
                request.DefaultSessionResrvations.Count != request.NumberOfSessionsPerWeek)
                errors.Add("Number of sessions per week doesn't match the sessions data");

            if (request.StartDate >= request.EndDate)
                errors.Add("End date can't be before the start date");

            if (request.StartDate < DateOnly.FromDateTime(DateTime.Now))
                errors.Add("Start date can't be in the past");

            if (request.EndDate <= DateOnly.FromDateTime(DateTime.Now))
                errors.Add("End date can't be before or equal to today");

            if (request.MaxNumberOfStudents <= 0)
                errors.Add("Group should allow at least one student");

            if (teacher == null)
                errors.Add("Teacher not found");

            if (teacher?.Subjects?.Any(s => s.ID == request.SubjectId) == false)
                errors.Add("Teacher doesn't have access to this subject");

            return new ReserveGroupResponseDTO
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }

    }
}