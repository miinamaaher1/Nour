using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
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
        private static readonly GeometryFactory _geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Adding Announcement Method
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


        // Reservation Methods 
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
            var validationResponse = ValidateOnlineGroupRequest(request, teacher);

            if (!validationResponse.IsValid)
                return validationResponse;

            // Creating the group 
            Group onlineGroup = new Group();

            // Assigning properties
            onlineGroup.SubjectID = request.SubjectId;
            onlineGroup.Address = null;
            onlineGroup.IsPrivate = request.IsPrivate;
            onlineGroup.IsOnline = true;
            onlineGroup.IsGirlsOnly = request.IsGirlsOnly;
            onlineGroup.IsActive = true;
            onlineGroup.CurrentStudents = 0;
            onlineGroup.TeacherID = request.TeacherId;
            onlineGroup.PricePerSession = request.PricePerSession;
            onlineGroup.MaxStudents = request.MaxNumberOfStudents;


            // Reserving sessions 
            onlineGroup.Sessions = new List<Session>();
            onlineGroup.GroupDefaults = new List<GroupDefaults>();
            foreach (var defaultSession in request.DefaultSessionResrvations!)
            {
                GroupDefaults gd = new GroupDefaults()
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    StartTime = defaultSession.StartTime,
                    EndTime = defaultSession.EndTime,
                    WeekDay = defaultSession.WeekDay
                };

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
                    onlineGroup.GroupDefaults.Add(gd);
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
        async public Task<ReserveGroupResponseDTO> ReserveOfflineLocalGroup(ReserveOfflineLocalGroupRequestDTO request)
        {
            if (request.TeacherId <= 0)
            {
                return new ReserveGroupResponseDTO
                {
                    IsValid = false,
                    Errors = ["Something went wrong! Invalid Teacher Id"]
                };
            }
            Point? parsedLocation = ParseLocation(request.Location);
            if (parsedLocation == null)
            {
                return new ReserveGroupResponseDTO
                {
                    IsValid = false,
                    Errors = ["Invalid Location please choose your location again!"]
                };
            }

            var response = new ReserveGroupResponseDTO();
            response.IsValid = true;
            response.Errors = new List<string>();

            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == request.TeacherId, ["Subjects"]);
            var validationResponse = ValidateOfflinLocalGroupRequest(request, teacher);

            if (validationResponse is not { IsValid: true })
            {
                return new ReserveGroupResponseDTO
                {
                    IsValid = false,
                    Errors = validationResponse?.Errors ?? ["Validation failed"]
                };
            }

            // Creating the group 
            Group offlineLocalGroup = new Group();

            // Assigning properties
            offlineLocalGroup.IsOnline = false;
            offlineLocalGroup.SubjectID = request.SubjectId;
            offlineLocalGroup.Address = new Address
            {
                Street = request.Street,
                City = request.City,
                Governorate = request.Governorate,
                Neighborhood = request.Neighborhood
            };
            offlineLocalGroup.Location = parsedLocation;
            offlineLocalGroup.IsPrivate = request.IsPrivate;
            offlineLocalGroup.IsGirlsOnly = request.IsGirlsOnly;
            offlineLocalGroup.IsActive = true;
            offlineLocalGroup.CurrentStudents = 0;
            offlineLocalGroup.TeacherID = request.TeacherId;
            offlineLocalGroup.PricePerSession = request.PricePerSession;
            offlineLocalGroup.MaxStudents = request.MaxNumberOfStudents;


            // Reserving sessions 
            offlineLocalGroup.Sessions = new List<Session>();
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
                        Location = parsedLocation,
                        IsOnline = false,
                        Address = new Address
                        {
                            Street = request.Street,
                            City = request.City,
                            Governorate = request.Governorate,
                            Neighborhood = request.Neighborhood
                        }
                    };

                    offlineLocalGroup.Sessions.Add(session);
                    current = current.AddDays(7);
                }
            }

            try
            {
                await _unitOfWork.Groups.AddAsync(offlineLocalGroup);
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
        public async Task<ReserveGroupResponseDTO> ReserveAnOfflineGroupInCenter(RequestOfflineGroupReservation request)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == request.TeacherId);
            var validationResult = await ValidateOfflineGroupRequest(request, teacher);

            if (!validationResult.IsValid)
                return validationResult;

            var center = await _unitOfWork.Centers.FindAsync(c => c.ID == request.CenterId, ["Address"]);

            var groupInCenter = new Group
            {
                SubjectID = request.SubjectId,
                Address = center.Address,
                IsPrivate = false,
                IsOnline = false,
                IsGirlsOnly = request.IsGirlsOnly,
                IsActive = true,
                CurrentStudents = 0,
                TeacherID = request.TeacherId,
                PricePerSession = request.PricePerSession,
                MaxStudents = request.Capacity,
                Sessions = new List<Session>(),
                GroupDefaults = new List<GroupDefaults>()
            };

            foreach (var session in request.Sessions)
            {
                var room = await _unitOfWork.Rooms.FindAsync(r => r.ID == session.RoomId);
                if (room == null)
                {
                    return new ReserveGroupResponseDTO
                    {
                        IsValid = false,
                        Errors = [$"Room with ID {session.RoomId} was not found."]
                    };
                }

                // create default sessions
                GroupDefaults groupDefaults = new GroupDefaults
                {
                    WeekDay = session.DayId,
                    StartDate = session.StartDate,
                    EndDate = session.EndDate,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    RoomID = room.ID,
                    Room = room,
                    Group = groupInCenter
                };
                groupInCenter.GroupDefaults.Add(groupDefaults);

                DateOnly current = session.StartDate;
                while (!MatchesWeekDay(current, session.DayId))
                {
                    current = current.AddDays(1);
                }

                while (current <= session.EndDate)
                {
                    var newSession = new Session
                    {
                        StartDateTime = current.ToDateTime(session.StartTime),
                        EndDateTime = current.ToDateTime(session.EndTime),
                        Duration = session.EndTime.ToTimeSpan() - session.StartTime.ToTimeSpan(),
                        Location = center.Location,
                        IsOnline = false,
                        Address = new Address
                        {
                            Street = center.Address!.Street,
                            City = center.Address!.City,
                            Governorate = center.Address!.Governorate,
                            Neighborhood = center.Address!.Neighborhood
                        }
                    };

                    var reservation = new RoomReservation
                    {
                        ReservationStatus = ReservationStatus.Pending,
                        Room = room,
                        RoomID = room.ID,
                        TotalPrice = room.PricePerHour,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        Session = newSession,
                        IsDeleted = false,
                        WeekDay = session.DayId
                    };

                    groupInCenter.Sessions.Add(newSession);
                    _unitOfWork.Sessions.Add(newSession);
                    _unitOfWork.RoomReservations.Add(reservation);

                    current = current.AddDays(7);
                }
            }

            try
            {
                await _unitOfWork.Groups.AddAsync(groupInCenter);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return new ReserveGroupResponseDTO
                {
                    IsValid = false,
                    Errors = ["Something went wrong while saving the data! Try again later."]
                };
            }

            return new ReserveGroupResponseDTO
            {
                IsValid = true,
                Errors = []
            };
        }


        // DB Utility Methods 
        public async Task<ICollection<GroupVM>> GetAllGroups(int teacherId)
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
        public async Task<ICollection<Subject>> GetMatchingSubjects(RequestSubjectDto request)
        {
            var teacherSubjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Teachers!.Any(t => t.ID == request.TeacherId));

            var filteredSubjects = teacherSubjects
                .Where(s => s.Year == request.Year && s.Semester == request.Semester)
                .ToList();

            return filteredSubjects;

        }
        public async Task<ICollection<Subject>> GetAllSubjects(RequestSubjectDto request)
        {
            var teacherSubjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Teachers!.Any(t => t.ID == request.TeacherId));
            return teacherSubjects.ToList();

        }
        public async Task<ICollection<ResponseCenterDto>> GetAllCentersPerGovernorate(string governorate)
        {
            var centers = await _unitOfWork.Centers.FindAllAsync(c =>
                c.Address != null &&
                c.Address.Governorate != null &&
                c.Address.Governorate.ToLower() == governorate.ToLower());

            return centers.Select(c => new ResponseCenterDto
            {
                CenterId = c.ID,
                CenterName = c.Name
            }).ToList();
        }
        public async Task<ICollection<Room>> GetAllAvailableRooms(RequestRoomDto request)
        {
            var allRoomsInTheCenter = await _unitOfWork.Rooms
                .FindAllAsync(r =>
                    r.CenterID == request.CenterID &&
                    r.Capacity >= request.Capacity &&
                    r.Equipment == Equipment.Lecture);

            var availableRooms = new List<Room>();

            foreach (var room in allRoomsInTheCenter)
            {
                bool isAvailable = await IsRoomAvailableAsync(
                    room.ID,
                    request.Day,
                    request.StartDate,
                    request.EndDate,
                    request.StartTime,
                    request.EndTime
                );

                if (isAvailable)
                {
                    availableRooms.Add(room);
                }
            }

            return availableRooms;
        }
        public async Task<ICollection<int>> GetAllMatchingYears(int teacherId)
        {
            List<int> years = new List<int>();
            if (teacherId <= 0)
            {
                return years;
            }
            var teacher = await _unitOfWork.Teachers.FindAsync(t => t.ID == teacherId, ["Subjects"]);

            var matchingYears = teacher.Subjects!
                .Select(s => (int)s.Year)
                .ToList();

            return matchingYears;
        }



        // Validation Methods
        private ReserveGroupResponseDTO ValidateOnlineGroupRequest(ReserveOnlineGroupRequestDTO request, Teacher? teacher)
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
        private ReserveGroupResponseDTO ValidateOfflinLocalGroupRequest(ReserveOfflineLocalGroupRequestDTO request, Teacher? teacher)
        {
            var errors = new List<string>();
            if (request.Governorate == null || request.Governorate == "")
            {
                errors.Add("Governorate can't be empty please provide the governorate");
            }
            if (request.City == null || request.City == "")
            {
                errors.Add("City can't be empty please provide the city");
            }
            if (request.Street == null || request.Street == "")
            {
                errors.Add("Street can't be empty please provide the street");
            }

            if (request.Street == null || request.Street == "")
            {
                errors.Add("Street can't be empty please provide the street");
            }

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
        private async Task<ReserveGroupResponseDTO> ValidateOfflineGroupRequest(RequestOfflineGroupReservation request, Teacher teacher)
        {
            var errors = new List<string>();

            // 1. Validate Sessions count
            if (request.Sessions == null || request.Sessions.Count != request.NumberOfSessions)
                errors.Add("Number of sessions doesn't match the provided session details.");

            // 2. Basic checks on sessions
            foreach (var session in request.Sessions)
            {
                if (session.StartDate >= session.EndDate)
                    errors.Add($"Session from {session.StartDate} to {session.EndDate} has an invalid date range.");

                if (session.StartTime >= session.EndTime)
                    errors.Add($"Session on {session.DayId} has an invalid time range from {session.StartTime} to {session.EndTime}.");

                if (session.StartDate < DateOnly.FromDateTime(DateTime.Now))
                    errors.Add($"Session start date {session.StartDate} cannot be in the past.");
            }

            // 3. Capacity
            if (request.Capacity <= 0)
                errors.Add("Capacity must be at least 1 student.");

            // 4. Price per session
            if (request.PricePerSession <= 0)
                errors.Add("Price per session must be greater than 0.");

            // 5. Teacher existence
            if (teacher == null)
                errors.Add("Teacher not found.");

            // 6. Teacher access to subject
            if (teacher?.Subjects?.Any(s => s.ID == request.SubjectId) == false)
                errors.Add("Teacher does not have access to the specified subject.");

            // 7. Validate room availability
            foreach (var session in request.Sessions)
            {
                bool isRoomAvailable = await IsRoomAvailableAsync(
                    session.RoomId,
                    session.DayId,
                    session.StartDate,
                    session.EndDate,
                    session.StartTime,
                    session.EndTime
                );

                if (!isRoomAvailable)
                {
                    errors.Add($"Room {session.RoomId} is not available on {session.DayId} between {session.StartTime} and {session.EndTime} from {session.StartDate} to {session.EndDate}.");
                }
            }

            return new ReserveGroupResponseDTO
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }


        // General Utility Methods
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
        private Point? ParseLocation(string? locationString)
        {
            if (string.IsNullOrWhiteSpace(locationString))
                return null;

            var parts = locationString.Split(',');

            if (parts.Length != 2)
                return null;

            if (double.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude) &&
                double.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude))
            {
                return _geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            }

            return null;
        }
        private async Task<bool> IsRoomAvailableAsync(int roomId, WeekDay day, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime)
        {
            var overlappingReservations = await _unitOfWork.RoomReservations.FindAllAsync(reservation =>
                reservation.RoomID == roomId &&
                (reservation.WeekDay & day) == day &&
                reservation.Date >= startDate && reservation.Date <= endDate &&
                (
                    startTime < reservation.EndTime && endTime > reservation.StartTime
                )
            );

            return !overlappingReservations.Any();
        }
        private bool IsOverlapping(TimeOnly s1, TimeOnly e1, TimeOnly s2, TimeOnly e2)
        {
            return (s1 < e2 && e1 > s2);
        }

        public Task<ICollection<string>> GetAllGovernorates()
        {
            throw new NotImplementedException();
        }
    }
}