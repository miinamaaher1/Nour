using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.CenterAdminViewModels
{
    public class ReservationViewModel
    {
        [Key]
        public int ID { get; set; }
        public int RoomID { get; set; }
        
        public decimal TotalPrice { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public WeekDay? WeekDay { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public int CenterId { set; get; }
        public string Name { get; set; }

        public int Capacity { get; set; }

        public Equipment? Equipment { get; set; }

        public string? CenterName { get; set; }

    }
}
