using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.ViewModels.Students;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

using XCourse.Core.ViewModels.StudentsViewModels;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;


namespace XCourse.Services.Implementations.StudentServices
{
    public class StudentGroupService:IStudentGroup
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration; 

        public StudentGroupService(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public GroupDetails Details(int id)
        {
            var group = _unitOfWork.Groups.Find(g=>g.ID == id,

            new string[] { "Address", "DefaultRoom", "Teacher", "Teacher.AppUser", "Subject" }


                );

            if (group == null)
            {
                return new GroupDetails();
            }


            GroupDetails details = new GroupDetails()
            {
                Id=group.ID,
                Address = group.Address,
                Location = group.Location,
                Key = _configuration["GoogleMaps:ApiKey"],
                DefaultSessionDays = group.DefaultSessionDays,
                DefaultRoom = group.DefaultRoom,
                Sessions = _unitOfWork.Sessions.FindAll(s=>s.GroupID==group.ID,new string[] { "RoomReservation.Room" },null,3).ToList(),
                Teacher = group.Teacher,
                IsActive = group.IsActive,
                IsOnline = group.IsOnline,
                IsPrivate = group.IsPrivate,
                CoverPicture = group.CoverPicture,
                Subject = group.Subject.Topic



            };

            return details;
        }

       

        public List< StudentGroup> GetStudentGroup(string id)
        {
            var student = _unitOfWork.Students.Find(
                s => s.AppUserID == id,
                new string[] { "Groups", "Groups.Teacher", "Groups.Teacher.AppUser", "Groups.Subject" }
            );
            if (student==null) return new List<StudentGroup> { };
            List<StudentGroup> studentGroups = new();
            foreach(var group in student.Groups)
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
    }
    }

