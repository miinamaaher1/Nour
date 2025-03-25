using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Core.DTOs
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public ClientInfo Client { get; set; }
        public string SuccessUrl { get; set; }
        public string FailureUrl { get; set; }
        public bool SaveToken { get; set; } = false; // For card tokenization
        public string Note { get; set; }
    }

    public class ClientInfo
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

}
