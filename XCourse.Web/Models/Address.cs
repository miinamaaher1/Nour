using System.ComponentModel.DataAnnotations.Schema;

namespace XCourse.Web.Models
{
    [ComplexType]
    public class Address
    {
        public string Street { get; set; }
        public string Neighbourhood { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
    }
}
