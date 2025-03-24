using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace XCourse.Core.Entities
{
    [Owned]
    public class Address
    {
        [MaxLength(25,ErrorMessage ="Number of characters for street must be less than or equal 25")]
        public string Street { get; set; }

        [MaxLength(25, ErrorMessage = "Number of characters for Neighbourhood must be less than or equal 25")]
        public string Neighbourhood { get; set; }

        [MaxLength(25, ErrorMessage = "Number of characters for City must be less than or equal 25")]
        public string City { get; set; }

        [MaxLength(25, ErrorMessage = "Number of characters for Governorate must be less than or equal 25")]
        public string Governorate { get; set; }
    }
}
