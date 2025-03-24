using Microsoft.EntityFrameworkCore;

namespace XCourse.Core.Entities
{
    [Owned]
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
