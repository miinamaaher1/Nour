using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class MapService : IMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public MapService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public MapInfoDTO InitializeMap(AppUser user)
        {
            MapInfoDTO mapInfo = new MapInfoDTO();

            mapInfo.Key = _configuration["GoogleMaps:ApiKey"];

            if (user.HomeLocation != null)
            {
                mapInfo.OriginX = user.HomeLocation.X;
                mapInfo.OriginY = user.HomeLocation.Y;
            }
            else
            {
                mapInfo.OriginX = 30.0444;
                mapInfo.OriginY = 31.2357;
            }

            return mapInfo;
        }
    }
}
