using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCourse.Services.Interfaces.PaymentService
{
    public interface IStripeService
    {
        public Task<string> CreateStripeSessionAsync(decimal amount, string userEmail);


        public  Task<bool> ProcessStripeSuccessAsync(string sessionId);



    }
}
