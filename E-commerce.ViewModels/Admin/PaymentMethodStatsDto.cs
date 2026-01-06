using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class PaymentMethodStatsDto
    {
        public string PaymentMethod { get; set; }   // COD, Card, Wallet, etc.
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
