using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class PaymentUpdateDto
    {
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public long Amount { get; set; }
        public string Status { get; set; } // Succeeded / Failed
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
