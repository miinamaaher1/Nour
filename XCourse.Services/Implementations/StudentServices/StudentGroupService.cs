using Microsoft.Extensions.Configuration;
using XCourse.Core.ViewModels.StudentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;
using XCourse.Core.Entities;


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

            new string[] { "Address", "DefaultRoom", "Teacher", "Teacher.AppUser", "Subject" }


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
                DefaultSessionDays = group.DefaultSessionDays,
                DefaultRoom = group.DefaultRoom,
                Sessions = _unitOfWork.Sessions.FindAll(s => s.GroupID == group.ID, new string[] { "RoomReservation.Room" }, null, 3).ToList(),
                Teacher = group.Teacher,
                IsActive = group.IsActive,
                IsOnline = group.IsOnline,
                IsPrivate = group.IsPrivate,
                CoverPicture = group.CoverPicture,
                Subject = group.Subject.Topic



            };

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
                    Address = group.Address,
                    DefaultSessionDays = group.DefaultSessionDays,
                    IsActive = group.IsActive,

                    IsPrivate = group.IsPrivate,
                    IsOnline = group.IsOnline,

                    CoverPicture = group.CoverPicture,
                    TeacherName = group.Teacher?.AppUser?.FirstName + " " + group.Teacher?.AppUser?.LastName,
                    Subject = group.Subject?.Topic






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

