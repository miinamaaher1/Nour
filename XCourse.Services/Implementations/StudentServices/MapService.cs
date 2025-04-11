using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.StudentServices;

namespace XCourse.Services.Implementations.StudentServices
{
    public class MapService : IMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public MapService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<MapInfoDTO> InitializeMapAsync(ClaimsPrincipal user)
        {
            AppUser auser = await _userManager.GetUserAsync(user);

            MapInfoDTO mapInfo = new MapInfoDTO();

            mapInfo.Key = _configuration["GoogleMaps:ApiKey"];

            if (auser.HomeLocation != null)
            {
                mapInfo.OriginX = auser.HomeLocation.X;
                mapInfo.OriginY = auser.HomeLocation.Y;
            }
            else
            {
                mapInfo.OriginX = 31.2357;
                mapInfo.OriginY = 30.0444;
            }

            return mapInfo;
        }

        public List<CenterMapDTO> SearchCenters(string query)
        {
            var centers = _unitOfWork.Centers.FindAll(c => c.Name.Contains(query)
                                                        || c.Address.Governorate.Contains(query)
                                                        || c.Address.City.Contains(query)
                                                        || c.Address.Neighborhood.Contains(query)
                                                        || c.Address.Street.Contains(query));

            List<CenterMapDTO> searchResult = new();

            if (centers != null && centers.Count() > 0)
            {
                foreach (var center in centers)
                {
                    if (center.Location != null)
                    {
                        CenterMapDTO res = new()
                        {
                            ID = center.ID,
                            Name = center.Name,
                            IsGirlsOnly = center.IsGirlsOnly,
                            Longitude = center.Location.X,
                            Latitude = center.Location.Y
                        };

                        if (center.Address !=  null)
                        {
                            res.Governorate = center.Address.Governorate;
                            res.City = center.Address.City;
                            res.Neighborhood = center.Address.Neighborhood;
                            res.Street = center.Address.Street;
                        }

                        searchResult.Add(res);
                    }
                }
            }

            return searchResult;
        }
    }
}