using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.CenterAdmins;
namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class EditRoomReservation
    {
        [Key]
        public int ID { get; set; }
        public int RoomID { set; get; }
     public string RoomName{  get; set; }
        public byte[]? PreviewPicture { get; set; }

        public List<EditRoomREservationDto> Rooms { set; get; } = new List<EditRoomREservationDto>();


    }
}
