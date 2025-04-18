using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.DTOs.CenterAdmins
{
    public class RoomDto
    {
        [Key]
        public int CenterId { get; set; }
        public int RoomId { set; get; }
        public string? Name { get; set; }

        public int Capacity { get; set; }
        [EnumDataType(typeof(Equipment))]
        public Equipment Equipment { get; set; }


        public decimal PricePerHour { get; set; }
        public  ICollection<RoomReservation>? RoomReservations { get; set; }

        public List<Equipment>? SelectedEquipments { get; set; } = new() ;
        
        public byte[]? PreviewPicture2 { get; set; }
    }
}
