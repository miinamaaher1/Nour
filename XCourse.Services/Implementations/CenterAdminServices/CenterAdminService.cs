using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.CenterAdminViewModels;
using XCourse.Core.DTOs.CenterAdmins;
using XCourse.Core.ViewModels.StudentsViewModels;
using XCourse.Infrastructure.Repositories.Interfaces;
using XCourse.Services.Interfaces.CenterAdminServices;
using NetTopologySuite.Geometries;

namespace XCourse.Services.Implementations.CenterAdminServices
{
    public class CenterAdminService : ICenterAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CenterAdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public int EditRoom(RoomDto room)
        {


            if (room == null) return 0;
            if (room.SelectedEquipments != null)
            {
                foreach (var item in room.SelectedEquipments)
                {
                    room.Equipment |= item;
                }
            }
         

            var Room = new Room()
                {
                    ID = room.RoomId,
                    CenterID = room.CenterId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    PricePerHour = room.PricePerHour,
                    PreviewPicture = room.PreviewPicture2,
                    Equipment = room.Equipment,
                    RoomReservations = room.RoomReservations
                };
                _unitOfWork.Rooms.Update(Room);
                return _unitOfWork.Save();
                
            

          
           
        }


        public RoomDto GetRoom(int id)
        {

            var room=_unitOfWork.Rooms.Find(R => R.ID == id,
                new string[] { "RoomReservations" }
                
                );

            if (room == null) return new RoomDto();

            RoomDto roomDto = new RoomDto()
            {
                Name=room.Name,
               RoomId=room.ID,
               CenterId=room.CenterID,
               Capacity=room.Capacity,
               PricePerHour=room.PricePerHour,
                PreviewPicture2 = room.PreviewPicture,
               Equipment=room.Equipment,
               RoomReservations=room.RoomReservations,
            };

            return roomDto;

        }






        public int AddNewRoom(RoomDto room)
        {
            if (room == null) return 0;
            if (room.SelectedEquipments != null)
            {
                foreach (var item in room.SelectedEquipments)
                {
                    room.Equipment |= item; 
                }
            }
            Room room1 = new Room()
            {
               CenterID=room.CenterId,
               Capacity=room.Capacity,
               Name=room.Name,
               PreviewPicture=room.PreviewPicture2,
               PricePerHour=room.PricePerHour,
               Equipment=room.Equipment,
               
               
            };
            _unitOfWork.Rooms.Add(room1);
         return   _unitOfWork.Save();

        }

        public List<RoomsViewModel> getAllRooms(int CenterId)
        {
            var center = _unitOfWork.Centers.Find(
                c => c.ID == CenterId,
                new string[] {"Rooms"}
            );

            if (center == null || center.Rooms == null)
            {
                return new List<RoomsViewModel>();
            }
            List<RoomsViewModel> Rooms = new();
            foreach(var room in center.Rooms)
            {
                Rooms.Add(
                new RoomsViewModel()
                {
                    CenterId=center.ID,
                    CenterName=center.Name,
                    RoomId=room.ID,
                    Name=room.Name,
                    Capacity=room.Capacity,
                    Equipment=room.Equipment,
                    PricePerHour=room.PricePerHour,
                });
            }
            return Rooms;
        }


        public int AddNewCenter(CreateCenterViewModel Center)
        {

           
            Center center = new Center()
            {
                Name = Center.Name,
                Address = Center.Address,
                IsGirlsOnly = Center.IsGirlsOnly,
                CenterAdminID=Center.CenterAdminid,
                Location= new Point(Center.Location.OriginX, Center.Location.OriginY) { SRID = 4326 },
                PreviewPicture=Center.PreviewPicture,

            };
            _unitOfWork.Centers.Add(center);
         return   _unitOfWork.Save();
        }

        public List<CenterViewModel> GetCenters(string id)
        {
            var CenterAdmin = _unitOfWork.CenterAdmins.Find(
                C => C.AppUserID == id, new string[] { "Centers", "Centers.Address" }
                );
            if (CenterAdmin == null)
            {
                return new List<CenterViewModel>();

            }
            List<CenterViewModel> centerViews = new();
            foreach(var item in CenterAdmin.Centers)
            {
                centerViews.Add(new CenterViewModel
                {
                 CenterAdminid=   CenterAdmin.ID,
                    CenterID = item.ID,
                    Name = item.Name,
                    Address = item.Address,
                    IsGirlsOnly = item.IsGirlsOnly,
                    PreviewPicture = item.PreviewPicture,
                   


                }

                    );
            }


            return centerViews;
        }

       
    }
}
