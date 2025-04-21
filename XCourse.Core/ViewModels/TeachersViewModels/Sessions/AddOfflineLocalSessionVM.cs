using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.TeachersViewModels.Sessions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using NetTopologySuite.Geometries;

    public class AddOfflineLocalSessionVM
    {
        [Required(ErrorMessage = "Group ID is required.")]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "Latitude is required.")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        public double? Longitude { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        [Column(TypeName = "geography")]
        public Point? Location { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public Address? Address { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string? Description { get; set; }
    }

}
