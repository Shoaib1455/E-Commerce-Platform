using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class CustomerStatsDto
    {
        public int TotalCustomers { get; set; }
        public int NewCustomersToday { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public int ReturningCustomers { get; set; }

        public List<TopCustomerDto> TopCustomers { get; set; }
    }
    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
