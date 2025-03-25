using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs
{
    public class PaymentResponse
    {
        public string Status { get; set; }
        public string CheckoutUrl { get; set; }
    }

}
