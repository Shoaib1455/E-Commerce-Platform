using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels
{
    public class CreatePaymentIntentDto
    {
        public int Amount { get; set; } // e.g. 5000 for $50
        public int OrderId { get; set; }
    }
}
