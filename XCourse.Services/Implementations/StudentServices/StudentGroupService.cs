using Microsoft.Extensions.Configuration;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;


namespace XCourse.Services.Implementations.StudentServices
{
    public class StudentGroupService : IStudentGroup
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public StudentGroupService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public GroupDetails Details(int id)
        {
            var group = _unitOfWork.Groups.Find(g => g.ID == id,
                new string[] { "Address", "DefaultRoom.Center", "Teacher.AppUser", "Subject", "GroupDefaults" }
            );

            if (group == null)
            {
                return new GroupDetails();
            }

            GroupDetails details = new GroupDetails()
            {
                Id = group.ID,
                Address = group.Address,
                Location = group.Location,
                Key = _configuration["GoogleMaps:ApiKey"],
                DefaultRoom = group.DefaultRoom,
                Sessions = _unitOfWork.Sessions.FindAll(s => s.GroupID == group.ID, new string[] { "RoomReservation.Room" }, null, 3).ToList(),
                Teacher = group.Teacher,
                IsOnline = group.IsOnline,
                IsPrivate = group.IsPrivate,
                CoverPicture = group.CoverPicture,
                Subject = group.Subject,
                PricePerSession = group.PricePerSession,
                Defaults = new List<DefaultTimeVM>()
            };

            foreach (var def in group.GroupDefaults)
            {
                var time = new DefaultTimeVM()
                {
                    WeekDay = def.WeekDay,
                    StartTime = def.StartTime,
                    EndTime = def.EndTime
                };
                details.Defaults.Add(time);
            }

            return details;
        }

        public List<StudentGroup> GetStudentGroup(string id)
        {
            var student = _unitOfWork.Students.Find(
                s => s.AppUserID == id,
                new string[] { "Groups", "Groups.Teacher", "Groups.Teacher.AppUser", "Groups.Subject" }
            );
            if (student == null) return new List<StudentGroup> { };
            List<StudentGroup> studentGroups = new();
            foreach (var group in student.Groups)
            {
                studentGroups.Add(new StudentGroup()
                {
                    GroupId = group.ID,
                    DefaultSessionDays = group.DefaultSessionDays,
                    IsGirlsOnly = group.IsGirlsOnly,

                    IsPrivate = group.IsPrivate,
                    IsOnline = group.IsOnline,

                    CoverPicture = group.CoverPicture,
                    TeacherName = group.Teacher?.AppUser?.FirstName + " " + group.Teacher?.AppUser?.LastName,
                    Subject = group.Subject?.Topic,

                    ProfilePicture = group.Teacher?.AppUser?.ProfilePicture
                });
            }
            return studentGroups;
        }

        public async Task<ICollection<RecommendedGroupViewModel>> RecommendedGroupService(string guid)
        {
            var student = await _unitOfWork.Students.FindAsync(
              x => x.AppUserID == guid,
              ["Groups", "AppUser"]
             );


            if (student == null) return new List<RecommendedGroupViewModel>();


            // Fetch groups based on the student's Major and Year with related data
            var groups = await _unitOfWork.Groups.FindAllAsync(
                 g => g.Subject != null &&
                     g.IsActive == true &&
                     g.Subject.Major == student.Major &&
                     g.Subject.Year == student.Year &&
                     g.IsPrivate == false &&
                     g.MaxStudents > g.CurrentStudents &&
                     !(g.IsGirlsOnly == true && student.AppUser.Gender == Gender.Male) &&
                     g.Address.City.ToLower() == student.AppUser.HomeAddress.City.ToLower() &&
                     !g.Students.Any(st => st.ID == student.ID),
                        ["Subject", "Students", "Teacher.AppUser"]
            );


            var recommendedGroups = groups
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
              CurrentStudentsCount = g.Students != null ? g.CurrentStudents : 0,
              TeacherProfilePicture = g.Teacher.AppUser != null ? g.Teacher.AppUser.ProfilePicture : null,
              GroupPicture = g.CoverPicture
          }).ToList();
            return await Task.FromResult(recommendedGroups.ToList());
        }
    }
}

