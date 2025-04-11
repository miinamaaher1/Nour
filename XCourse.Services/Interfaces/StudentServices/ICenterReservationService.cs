using System.Security.Claims;
using XCourse.Core.DTOs;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.ViewModels.StudentsViewModels;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface ICenterReservationService
    {
        Task<CenterVM> GetCenterDetailsAsync(int id, ClaimsPrincipal user);
        List<ReservationTimeDTO> GetDayReservations(int id, DateOnly date);
        Task<RequestStatusDTO> ReserveRoomAsync(int roomId, DateOnly date, TimeOnly start, TimeOnly end, ClaimsPrincipal user);
    }
}
