using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class EnrollStudentService : IEnrollStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EnrollStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
