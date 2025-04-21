using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.DTOs.StudentDTOs;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels.Sessions
{
    public class EditOfflineLocalSessionVM
    {
        public int SessionID { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Address? Address { get; set; }
        public string? Description { get; set; }
        public MapInfoDTO Location { set; get; }
    }
}
