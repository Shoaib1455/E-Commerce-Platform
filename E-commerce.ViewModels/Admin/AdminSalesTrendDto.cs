using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class AdminSalesTrendDto
    {
        public string Period { get; set; }   // e.g. "2025-01" or "2025-01-10"
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
    }
}
