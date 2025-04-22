using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Core.Entities;

using XCourse.Services.Interfaces.CenterAdminServices;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using Microsoft.AspNetCore.JsonPatch.Internal;

namespace XCourse.Services.Implementations.CenterAdminServices
{
    public class CenterAdminHomeService : ICenterAdminHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CenterAdminHomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




     


        public async Task<List<RoomHomeViewModel>> GetAllRooms(string id)
        {
            var CenterAdmin = await _unitOfWork.CenterAdmins.FindAsync(
                x => x.AppUserID == id,
                new string[] { "Centers", "Centers.Rooms", "Centers.Rooms.RoomReservations" });
            if (CenterAdmin?.Centers == null)
            {
                return new List<RoomHomeViewModel>();
            }

            var validCenters = CenterAdmin.Centers
                .Where(c => c.Rooms != null && c.Rooms.Any(r => r.RoomReservations.Any()))
                .ToList();

            if (!validCenters.Any())
            {
                return new List<RoomHomeViewModel>();
            }
            


            List<RoomHomeViewModel> roomHomeViews = new List<RoomHomeViewModel>();

            foreach (var center in CenterAdmin.Centers)
            {

                roomHomeViews.Add
                    (new RoomHomeViewModel()
                    {
                        CenterId = center.ID,
                        CenterName = center.Name,
                        TotalRooms = center?.Rooms?.Count ?? 0,
                        BookedRooms = center?.Rooms?.Count(r => r.RoomReservations.Any(res => res.ReservationStatus == ReservationStatus.Approved)) ?? 0




                    });
                

            }

            return roomHomeViews;


        }

        public async Task<List<TopCenterViewModel>> GetTopCenter(string id)
        {
            var CenterAdmin = await _unitOfWork.CenterAdmins.FindAsync(
                x => x.AppUserID == id,
                new string[] { "Centers", "Centers.Rooms", "Centers.Rooms.RoomReservations" });

            if (CenterAdmin?.Centers == null)
            {
                return new List<TopCenterViewModel>();
            }

            var validCenters = CenterAdmin.Centers
                .Where(c => c.Rooms != null && c.Rooms.Any(r => r.RoomReservations.Any()))
                .ToList();

            if (!validCenters.Any())
            {
                return new List<TopCenterViewModel>();
            }

            List<TopCenterViewModel> topCenterViews = new List<TopCenterViewModel>();
            foreach (var center in CenterAdmin.Centers)
            {
                topCenterViews.Add(
                    new TopCenterViewModel
                    {
                        centerID = center.ID,
                        CenterName = center.Name,
                        TotalBookings = center.Rooms.Sum(r => r.RoomReservations.Count())
                    });
            }

            return topCenterViews;
        }
    }
}
