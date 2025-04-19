using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Core.ViewModels.PaymentViewModels
{
    public class PaymentDetailsVM
    {
        public string UserName { get; set; }
        public decimal Balance { get; set; }
        public string? Toast { get; set; }
        public List<PaymentHistoryVM>? PaymentHistory { get; set; } = new List<PaymentHistoryVM>();

    }



    public class PaymentHistoryVM
    {

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Status { get; set; }
    }
}
