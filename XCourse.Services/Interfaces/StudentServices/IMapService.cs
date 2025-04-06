using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;

namespace XCourse.Services.Interfaces.StudentServices
{
    public interface IMapService
    {
        MapInfoDTO InitializeMap(AppUser user);
    }
}
