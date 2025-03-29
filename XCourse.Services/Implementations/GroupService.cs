using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.Teacher;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.Teacher;

namespace XCourse.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ResponseCenterDto>> GetAllCentersPerGovernorate(string governorate)
        {
            var centers = await _unitOfWork.Centers.FindAllAsync(c =>
                c.Address != null &&
                c.Address.Governorate != null &&
                c.Address.Governorate.ToLower() == governorate.ToLower());

            return centers.Select(c => new ResponseCenterDto
            {
                CenterId = c.ID,
                CenterName = c.Name
            });
        }
        public async Task<IEnumerable<Room>> GetAllAvailableRooms(RequestRoomDto request)
        {
            var allRoomsInTheCenter = await _unitOfWork.Rooms
                .FindAllAsync(r => r.CenterID == request.CenterID && r.Capacity >= request.Capacity);

            var allUnavailableRoomsReservations = await _unitOfWork.RoomReservations
                .FindAllAsync(reservation =>
                    (reservation.WeekDay & request.Day) == request.Day &&
                    reservation.Date >= request.StartDate && reservation.Date <= request.EndDate && 
                    (
                        (request.StartTime >= reservation.StartTime && request.StartTime < reservation.EndTime) ||  
                        (request.EndTime > reservation.StartTime && request.EndTime <= reservation.EndTime) ||      
                        (request.StartTime <= reservation.StartTime && request.EndTime >= reservation.EndTime)       
                    )
                );


            var unavailableRoomIds = allUnavailableRoomsReservations
                .Select(r => r.RoomID)
                .Distinct()
                .ToHashSet();


            var availableRooms = allRoomsInTheCenter
                .Where(room => !unavailableRoomIds.Contains(room.ID))
                .ToList();

            return availableRooms;
        }
        public async Task<IEnumerable<string>> GetAllGovernorates()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Subject>> GetMatchingSubjects(RequestSubjectDto request)
        {
            var subjects = await _unitOfWork.Subjects.FindAllAsync(s => s.Year == request.Year && s.Semester == request.Semester);
            return subjects;
        }
    }
}