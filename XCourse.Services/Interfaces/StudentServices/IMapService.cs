using System.Security.Claims;
using XCourse.Core.DTOs.StudentDTOs;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface IMapService
    {
        Task<MapInfoDTO> InitializeMapAsync(ClaimsPrincipal user);
        List<CenterMapDTO> SearchCenters(string query);
    }
}
