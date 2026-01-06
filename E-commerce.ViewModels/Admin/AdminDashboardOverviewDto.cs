using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Admin
{
    public class AdminDashboardOverviewDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }

        public int TotalOrders { get; set; }
        public int TodayOrders { get; set; }

        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }

        public int TotalCustomers { get; set; }

        public decimal AverageOrderValue { get; set; }
    }
}
