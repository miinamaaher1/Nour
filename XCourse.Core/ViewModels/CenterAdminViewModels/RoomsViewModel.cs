using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XCourse.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class RoomsViewModel
    {
        [Key]
        public int CenterId { get; set; }
       public int RoomId { set; get; }
        public string Name { get; set; }

        public int Capacity { get; set; }
        [EnumDataType(typeof(Equipment))]
        public Equipment Equipment { get; set; }

    
        public decimal PricePerHour { get; set; }

       

       
    }
}
