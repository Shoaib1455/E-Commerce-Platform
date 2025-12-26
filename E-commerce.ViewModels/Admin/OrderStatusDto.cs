using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class OrderStatusDto
    {
        public string Status { get; set; }   // Pending, Processing, Delivered, Cancelled, etc.
        public int TotalOrders { get; set; }
    }
}
