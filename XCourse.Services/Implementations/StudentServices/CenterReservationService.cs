using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using XCourse.Core.DTOs;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.PaymentService;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class CenterReservationService : ICenterReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITransactionService _transactionService;

        public CenterReservationService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IConfiguration configuration,ITransactionService transactionService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _transactionService = transactionService;
        }

        public async Task<CenterVM> GetCenterDetailsAsync(int id, ClaimsPrincipal user)
        {
            AppUser auser = await _userManager.GetUserAsync(user);

            var userWallet = await _unitOfWork.Wallets.FindAsync(w => w.AppUserID == auser.Id);

            var center = _unitOfWork.Centers.Get(id);

            CenterVM centerVM = new CenterVM()
            {
                ID = center.ID,
                Name = center.Name,
                Governorate = center.Address.Governorate,
                City = center.Address.City,
                Neighborhood = center.Address.Neighborhood,
                Street = center.Address.Street,
                PreviewPicture = center.PreviewPicture,
                IsGirlsOnly = center.IsGirlsOnly,
                Location = center.Location,
                MapKey = _configuration["GoogleMaps:ApiKey"],
                WalletBalance = userWallet.Balance,
                AvailbleRooms = new List<RoomVM>(),
                Description = center.Description
            };

            if (auser.Gender == Gender.Male && center.IsGirlsOnly)
            {
                centerVM.IncompatibleGender = true;
            }
            else
            {
                centerVM.IncompatibleGender = false;
            }

            if (centerVM.IncompatibleGender) return centerVM;

            var rooms = _unitOfWork.Rooms.FindAll(r => r.CenterID == center.ID && (r.Equipment.HasFlag(Equipment.Study) || r.Equipment.HasFlag(Equipment.Meeting)));

            foreach (var room in rooms)
            {
                RoomVM roomVM = new RoomVM()
                {
                    ID = room.ID,
                    Name = room.Name,
                    Equipment = room.Equipment,
                    Capacity = room.Capacity,
                    PricePerHour = room.PricePerHour,
                    PreviewPicture = room.PreviewPicture,
                    Description = room.Description

                };
                centerVM.AvailbleRooms.Add(roomVM);
            }
            return centerVM;
        }

        public List<ReservationTimeDTO> GetDayReservations(int id, DateOnly date)
        {
            List<ReservationTimeDTO> result = new List<ReservationTimeDTO>();

            var reservations = _unitOfWork.RoomReservations.FindAll(r => r.Date == date && r.RoomID == id);

            foreach (var reservation in reservations)
            {
                ReservationTimeDTO res = new()
                {
                    Start = ((TimeOnly)reservation.StartTime!).ToString("HH:mm"),
                    End = ((TimeOnly)reservation.EndTime).ToString("HH:mm")
                };
                result.Add(res);
            }
            return result;
        }

        public async Task<RequestStatusDTO> ReserveRoomAsync(int roomId, DateOnly date, TimeOnly start, TimeOnly end, ClaimsPrincipal user)
        {
            RequestStatusDTO status = new RequestStatusDTO();
            try
            {

                var auser = await _userManager.GetUserAsync(user);

                var room = _unitOfWork.Rooms.Find(r=>r.ID==roomId, ["Center"]);

                var centerAdmin= _unitOfWork.CenterAdmins.Find(c=>c.ID == room.Center.CenterAdminID, ["AppUser"]);

                var total = (decimal)(end - start).TotalHours * room.PricePerHour;


                var reservations = _unitOfWork.RoomReservations.FindAll(r => r.Date == date && r.RoomID == roomId);

                if (reservations.Any(r => r.StartTime > start && r.StartTime < end || r.EndTime > start && r.EndTime < end))
                {
                    status.IsValid = false;
                    status.Errors = ["Unavailable Reservation, something went wrong..."];
                    return status;
                }

                var stud = _unitOfWork.Students.Find(s => s.AppUserID == auser.Id);

                RoomReservation rr = new()
                {
                    RoomID = roomId,
                    Date = date,
                    StartTime = start,
                    EndTime = end,
                    StudentID = stud.ID,
                    TotalPrice = total,
                    ReservationStatus = ReservationStatus.Approved
                };

                var IsPaid= await _transactionService.MakeTransactionAsync (
                    auser.Id,
                    centerAdmin.AppUserID,
                    total,
                    TransactionType.Payment
                );
                if (!IsPaid)
                {
                    status.IsValid = false;
                    status.Errors = ["Payment failed"];
                    return status;
                }
                _unitOfWork.RoomReservations.Add(rr);
                _unitOfWork.Save();
                status.IsValid = true;
                status.Errors = new List<string>();
                return status;
            }
            catch (Exception ex)
            {
                status.IsValid = false;
                status.Errors = new List<string>(["Internal server error"]);
                return status;
            }
        }
    }
}
