using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
