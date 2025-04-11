using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class EnrollStudentService : IEnrollStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public EnrollStudentService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<DetailedGroupVM> GetGroupInfo(int groupID, ClaimsPrincipal user)
        {
            var group = _unitOfWork.Groups.Find(g => g.ID == groupID, ["Subject", "Teacher.AppUser", "GroupDefaults", "DefaultRoom.Center"]);
            if (group == null) return null;

            var auser = await _userManager.GetUserAsync(user);
            var stud = _unitOfWork.Students.Find(s => s.AppUserID == auser.Id);

            DetailedGroupVM groupVM = new DetailedGroupVM()
            {
                ID = group.ID,
                CoverPicture = group.CoverPicture,

                PricePerSession = group.PricePerSession,
                MaxStudents = group.MaxStudents,
                CurrentStudents = group.CurrentStudents,

                SubjectName = group.Subject.Topic,
                Year = group.Subject.Year,
                Semester = group.Subject.Semester,
                Major = group.Subject.Major,

                TeacherName = group.Teacher.AppUser.FirstName + " " + group.Teacher.AppUser.LastName,
                TeacherProfilePicture = group.Teacher.AppUser.ProfilePicture,

                StudentID = stud.ID,
                StudentGender = auser.Gender,

                MapKey = _configuration["GoogleMaps:ApiKey"],
                Location = group.Location,
                Governorate = group.Address.Governorate,
                City = group.Address.City,
                Neighborhood = group.Address.Neighborhood,
                Street = group.Address.Street,

                IsOnline = group.IsOnline,
                IsPrivate = group.IsPrivate,
                IsGirlsOnly = group.IsGirlsOnly,

                DefaultTimes = new List<DefaultTimeVM>()
            };

            if (group.DefaultRoom != null)
            {
                groupVM.CenterName = group.DefaultRoom.Center.Name;
                groupVM.Location = group.DefaultRoom.Center.Location;
                groupVM.Governorate = group.DefaultRoom.Center.Address.Governorate;
                groupVM.City = group.DefaultRoom.Center.Address.City;
                groupVM.Neighborhood = group.DefaultRoom.Center.Address.Neighborhood;
                groupVM.Street = group.DefaultRoom.Center.Address.Street;
            }

            foreach (var def in group.GroupDefaults)
            {
                var time = new DefaultTimeVM()
                {
                    WeekDay = def.WeekDay,
                    StartTime = def.StartTime,
                    EndTime = def.EndTime
                };
                groupVM.DefaultTimes.Add(time);
            }

            return groupVM;
        }

        public bool Enroll(int studentID, int groupID)
        {
            try
            {
                var group = _unitOfWork.Groups.Find(g => g.ID == groupID, ["Students", "Sessions"]);
                if (group.Students.Any(s => s.ID == studentID) || group.CurrentStudents == group.MaxStudents || group.IsActive == false) throw new Exception();


                var student = _unitOfWork.Students.Find(s => s.ID == studentID, ["AppUser"]);
                if (student.AppUser.Gender == Gender.Male && group.IsGirlsOnly) throw new Exception();

                var nextSession = group.Sessions.OrderBy(s => s.StartDateTime).FirstOrDefault(s => s.StartDateTime > DateTime.Now);

                var wallet = _unitOfWork.Wallets.Find(w => w.AppUserID == student.AppUserID);

                if (wallet.Balance < group.PricePerSession)
                {
                    return false;
                }

                Transaction transaction = new Transaction()
                {
                    Amount = group.PricePerSession,
                    CreatedAt = DateTime.Now,
                    WalletID = wallet.ID,
                    Type = TransactionType.Deposit,
                    PaymentTransactionID = "xxx"
                };

                wallet.Balance -= group.PricePerSession;

                group.Students.Add(student);
                group.CurrentStudents++;

                Attendance att = new Attendance()
                {
                    StudentID = student.ID,
                    SessionID = nextSession.ID,
                    HasPaid = true
                };

                _unitOfWork.Transactions.Add(transaction);
                _unitOfWork.Attendances.Add(att);

                _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
