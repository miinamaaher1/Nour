using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.CenterAdmins;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.CenterAdminViewModels
{

    public class TransformReservationItem
    {
        public int ReservationId { get; set; }
        public DateOnly? ReservationDate { get; set; }
        public WeekDay? WeekDay { get; set; }
        public int SelectedNewRoomId { get; set; }

        
        public List<NewRoomDto> AvailableRooms { get; set; }
    }
    public class transfomReservations
    {
        public List<TransformReservationItem> ApproveReservations { get; set; }
        public int RoomID { set; get; }
        public string?RoomName { get; set; }
        public int? Capacity { get; set; }
        [EnumDataType(typeof(Equipment))]
        public Equipment? Equipment { get; set; }

        public int CenterId {  set; get; }

    }
}
