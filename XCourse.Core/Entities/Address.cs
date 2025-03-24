using Microsoft.EntityFrameworkCore;

namespace XCourse.Core.Entities
{
    [Owned]
    public class Address
    {
        public string Street { get; set; }
        public string Neighbourhood { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
    }
}
