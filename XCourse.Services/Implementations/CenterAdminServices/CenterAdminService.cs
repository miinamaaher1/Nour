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
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.DTOs.CenterAdmins;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XCourse.Services.Implementations.CenterAdminServices
{
    public class CenterAdminService : ICenterAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public CenterAdminService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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

            var room = _unitOfWork.Rooms.Find(R => R.ID == id,
                new string[] { "RoomReservations" }

                );

            if (room == null) return new RoomDto();

            RoomDto roomDto = new RoomDto()
            {
                Name = room.Name,
                RoomId = room.ID,
                CenterId = room.CenterID,
                Capacity = room.Capacity,
                PricePerHour = room.PricePerHour,
                PreviewPicture2 = room.PreviewPicture,
                Equipment = room.Equipment,
                RoomReservations = room.RoomReservations
    .Where(r => r.ReservationStatus == ReservationStatus.Approved)
    .ToList(),
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
                CenterID = room.CenterId,
                Capacity = room.Capacity,
                Name = room.Name,
                PreviewPicture = room.PreviewPicture2,
                PricePerHour = room.PricePerHour,
                Equipment = room.Equipment,


            };
            _unitOfWork.Rooms.Add(room1);
            return _unitOfWork.Save();

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
            foreach (var room in center.Rooms)
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
            if (Center == null) return 0;

            Center center = new Center()
            {
                Name = Center.Name,
                Address = Center.Address,
                IsGirlsOnly = Center.IsGirlsOnly,
                CenterAdminID = Center.CenterAdminid,
                Location = new Point(Center.Location.OriginX, Center.Location.OriginY) { SRID = 4326 },
                PreviewPicture = Center.PreviewPicture,

            };
            _unitOfWork.Centers.Add(center);
            return _unitOfWork.Save();
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
            foreach (var item in CenterAdmin.Centers)
            {
                centerViews.Add(new CenterViewModel
                {
                    CenterAdminid = CenterAdmin.ID,
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

        public CreateCenterViewModel GetCenter(int id)
        {
            var center = _unitOfWork.Centers.Find(c => c.ID == id, new string[] { "Address" });

            if (center == null) return new CreateCenterViewModel();

            var EditCenter = new CreateCenterViewModel()
            {
                CenterID = center.ID,
                Name = center.Name,
                Address = center.Address,
                Location = new MapInfoDTO()
                {
                    Key = _configuration["GoogleMaps:ApiKey"],
                    OriginX = center.Location.X,
                    OriginY = center.Location.Y,

                },
                IsGirlsOnly = center.IsGirlsOnly,
                CenterAdminid = center.CenterAdminID,
                PreviewPicture = center.PreviewPicture

            };

            return EditCenter;
        }

        public int EditCenter(CreateCenterViewModel Center)
        {
            if (Center == null) return 0;

            var EditCenter = new Center()
            {
                Name = Center.Name,
                Address = Center.Address,
                Location = new Point(Center.Location.OriginX, Center.Location.OriginY) { SRID = 4326 },
                IsGirlsOnly = Center.IsGirlsOnly,
                PreviewPicture = Center.PreviewPicture,
                ID = Center.CenterID,
                CenterAdminID = Center.CenterAdminid

            };

            _unitOfWork.Centers.Update(EditCenter);
            return _unitOfWork.Save();
        }

        public List<ReservationViewModel> GetReservations(int id)
        {
            var Room = _unitOfWork.Rooms.Find(
                R => R.ID == id, new string[] { "RoomReservations" });


            if (Room == null) return new List<ReservationViewModel>();

            List<ReservationViewModel> Reservations = new List<ReservationViewModel>();
            foreach (var reservation in Room.RoomReservations)
            {
                Reservations.Add(
                      new ReservationViewModel
                      {
                          Name = Room.Name,
                          Capacity = Room.Capacity,
                          Equipment = Room.Equipment,
                          RoomID = Room.ID,

                          CenterId = Room.CenterID,

                          ID = reservation.ID,
                          Date = reservation.Date,
                          WeekDay = reservation.WeekDay,
                          StartTime = reservation.StartTime,
                          EndTime = reservation.EndTime,
                          TotalPrice = reservation.TotalPrice,
                          ReservationStatus = reservation.ReservationStatus




                      });


            }


            return Reservations;

        }

        public int ApproveReservation(int id)
        {
            var Reservation = _unitOfWork.RoomReservations.Find(r => r.ID == id);
            if (Reservation == null) return 0;

            Reservation.ReservationStatus = ReservationStatus.Approved;

            _unitOfWork.RoomReservations.Update(Reservation);
            _unitOfWork.Save();
            return Reservation.RoomID;
        }

        public int RejectReservation(int id)
        {
            var Reservation = _unitOfWork.RoomReservations.Find(r => r.ID == id);
            if (Reservation == null) return 0;

            Reservation.ReservationStatus = ReservationStatus.Declined;

            _unitOfWork.RoomReservations.Update(Reservation);
            _unitOfWork.Save();
            return Reservation.RoomID;
        }

        public DetailsReservationViewModel DetailsReservation(int id)
        {
            var Reservation = _unitOfWork.RoomReservations.Find(R => R.ID == id,

                new string[] { "Room" });


            if (Reservation == null) return new DetailsReservationViewModel();


            var Details = new DetailsReservationViewModel()
            {
                RoomID = Reservation.RoomID,
                ID = Reservation.ID,
                Name = Reservation.Room.Name,
                EndTime = Reservation.EndTime,
                StartTime = Reservation.StartTime,
                WeekDay = Reservation.WeekDay,
                TotalPrice = Reservation.TotalPrice,
                Equipment = Reservation.Room.Equipment,
                Capacity = Reservation.Room.Capacity,
                Date = Reservation.Date,
                PreviewPicture = Reservation.Room.PreviewPicture
            };
            return Details;

        }

        public EditRoomReservation EditRoomReservation(int ReservationID)
        {

            var Reservation = _unitOfWork.RoomReservations.Find(r => r.ID == ReservationID, new string[] { "Room", "Room.RoomReservations" });

            if (Reservation == null)
                return new EditRoomReservation();


            var Center = _unitOfWork.Centers.Find(
                c => c.ID == Reservation.Room.CenterID,
                new string[] { "Rooms", "Rooms.RoomReservations" }
            );


            var correctRooms = Center.Rooms
                .Where(r => r.Equipment == Reservation.Room.Equipment &&
                            r.Capacity >= Reservation.Room.Capacity);
                

            var availableRoomsList = correctRooms
       .Where(room =>

           room.RoomReservations == null ||
           room.RoomReservations
               .Where(res => res.WeekDay == Reservation.WeekDay && res.ID != Reservation.ID)
               .All(res =>
                   res.EndTime <= Reservation.StartTime ||
                   res.StartTime >= Reservation.EndTime
               )
       )
       .Select(room => new EditRoomREservationDto
       {
           RoomID = room.ID,
           RoomName = room.Name
       })
       .ToList();


            var result = new EditRoomReservation
            {
                ID = Reservation.ID,
                RoomID = Reservation.RoomID,
                RoomName = Reservation.Room.Name,
                PreviewPicture = Reservation.Room.PreviewPicture,
                Rooms = availableRoomsList
            };

            return result;

        }

        public int UpdateReservtion(EditRoomReservation editRoom)
        {
            var Reservation = _unitOfWork.RoomReservations.Find(r => r.ID == editRoom.ID,

                new string[] { "Room" }
                );
            if (Reservation == null) return 0;

            var newroom = _unitOfWork.Rooms.Find(r => r.ID == editRoom.RoomID);
            Reservation.RoomID = editRoom.RoomID;
            Reservation.Room = newroom;
            _unitOfWork.RoomReservations.Update(Reservation);
            _unitOfWork.Save();
            return Reservation.RoomID;


        }



        public transfomReservations transfomReservations(int RoomId, int CenterId)
        {
            var Center = _unitOfWork.Centers.Find(c => c.ID == CenterId, new string[] { "Rooms", "Rooms.RoomReservations" });
            var Room = _unitOfWork.Rooms.Find(r => r.ID == RoomId, new string[] { "RoomReservations" });

            var correctRooms = Center.Rooms
                .Where(r => r.ID != RoomId &&
                            r.Capacity >= Room.Capacity &&
                            (r.Equipment & Room.Equipment) == Room.Equipment);


            List<TransformReservationItem> AvalibleRoom = new();
            foreach (var item in Room.RoomReservations)
            {
                var availableRoom = correctRooms
                    .Where(r => r.RoomReservations
                        .Where(res => res.WeekDay == item.WeekDay &&res.ID != item.ID)
                        .All(res =>
                            res.EndTime <= item.StartTime ||
                            res.StartTime >= item.EndTime)).ToList();

                AvalibleRoom.Add(new TransformReservationItem
                {
                    ReservationId = item.ID,
                    AvailableRooms = availableRoom.Select(room => new NewRoomDto
                    {
                        NewRoomID = room.ID,
                        RoomName = room.Name,
                        Capacity=room.Capacity,
                        Equipment=room.Equipment
                        
                    }).ToList(),
                    ReservationDate=item.Date,
                    WeekDay=item.WeekDay,
                   
                    

                   
                });


            }

            var RoomList = new transfomReservations()
            {
                RoomID=Room.ID,
                RoomName=Room.Name,
                Capacity=Room.Capacity,
                Equipment=Room.Equipment,
                ApproveReservations=AvalibleRoom,
                CenterId=Room.CenterID,
            };

            return RoomList;

        }

        public int ConfirmTransformRoom(transfomReservations transfom)
        {

            var Room = _unitOfWork.Rooms.Find(r => r.ID == transfom.RoomID, new string[] { "RoomReservations" });
            var pendingreservations = Room.RoomReservations.Where(r => r.ReservationStatus == ReservationStatus.Declined || r.ReservationStatus == ReservationStatus.Pending || (r.ReservationStatus == ReservationStatus.Approved && r.Date < DateOnly.FromDateTime(DateTime.Now))).ToList();

            _unitOfWork.RoomReservations.DeleteRange(pendingreservations);

            var FuturePendingReservations = Room.RoomReservations.Where(r => (r.ReservationStatus == ReservationStatus.Pending || r.ReservationStatus == ReservationStatus.Declined) && r.Date < DateOnly.FromDateTime(DateTime.Now)).ToList();
            _unitOfWork.RoomReservations.DeleteRange(FuturePendingReservations);
           
            if (transfom.ApproveReservations.Any())
            {
                foreach (var item in transfom.ApproveReservations)
                {
                    var r = _unitOfWork.RoomReservations.Find(res => res.ID == item.ReservationId, new string[] { "Room" });

                    r.RoomID = item.SelectedNewRoomId;
                    var newroom = _unitOfWork.Rooms.Get(item.SelectedNewRoomId);
                    r.Room = newroom;

                }

                _unitOfWork.Save();
                _unitOfWork.Rooms.Delete(Room);
                _unitOfWork.Save();
                return 1;
            }

            else return 0;

        }

        public int DeleteRoom(RoomDto room)
        {
            var Room = _unitOfWork.Rooms.Find(r => r.ID == room.RoomId, new string[] { "RoomReservations" });
            if (Room == null) return 0;
            var pendingreservations= Room.RoomReservations.Where(r => r.ReservationStatus == ReservationStatus.Declined|| r.ReservationStatus == ReservationStatus.Pending|| (r.ReservationStatus==ReservationStatus.Approved&&r.Date < DateOnly.FromDateTime(DateTime.Now))).ToList();
           
            _unitOfWork.RoomReservations.DeleteRange(pendingreservations);

            var FuturePendingReservations = Room.RoomReservations.Where(r => (r.ReservationStatus == ReservationStatus.Pending || r.ReservationStatus == ReservationStatus.Declined) && r.Date < DateOnly.FromDateTime(DateTime.Now)).ToList();
            _unitOfWork.RoomReservations.DeleteRange(FuturePendingReservations);


            var approveReservations = Room.RoomReservations.Where(r => r.ReservationStatus == ReservationStatus.Approved)
                .Where(r => r.Date >= DateOnly.FromDateTime(DateTime.Now)).ToList();

            if(!approveReservations.Any())
            {
                _unitOfWork.Save();
                _unitOfWork.Rooms.Delete(Room);
                _unitOfWork.Save();
                return 1;


            }
            return 0;
            


            


           
        }

    }
}
