using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    [NotMapped]
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
