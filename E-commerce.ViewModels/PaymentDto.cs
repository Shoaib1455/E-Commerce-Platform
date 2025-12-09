using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class PaymentDto
    {
        public class PaymentEvent
        {
            public string EventType { get; set; }
            public PaymentData Data { get; set; }
        }

        public class PaymentData
        {
            public string TransactionId { get; set; }
            public string Status { get; set; }
            public decimal Amount { get; set; }
            public string OrderId { get; set; }
        }
    }
}
