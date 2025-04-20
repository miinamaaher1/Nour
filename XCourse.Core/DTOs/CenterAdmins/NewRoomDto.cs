using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.DTOs.CenterAdmins
{
    public class NewRoomDto
    {
        public int NewRoomID { get; set; }
        
        public string RoomName { get; set; }

        public int? Capacity { get; set; }
        [EnumDataType(typeof(Equipment))]
        public Equipment? Equipment { get; set; }

        
    }
}
