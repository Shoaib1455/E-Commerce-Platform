using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.ViewModels.Seller
{
    public class SellerDashboardDto
    {
    }

    public class SellerDashboardOverviewDto
    {
        public SalesOrdersSection SalesOrders { get; set; }
        public ProductSection Products { get; set; }
        public CustomerSection Customers { get; set; }
        public PaymentSection Payments { get; set; }
        public OtherMetricsSection OtherMetrics { get; set; }
    }

    /* -------------------- SECTIONS -------------------- */

    public class SalesOrdersSection
    {
        public decimal AllTimeSales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal TodaySales { get; set; }

        public int AllTimeOrders { get; set; }
        public int MonthlyOrders { get; set; }
        public int TodayOrders { get; set; }

        public OrderStatusBreakdown OrderStatus { get; set; }
    }

    public class OrderStatusBreakdown
    {
        public int Pending { get; set; }
        public int Shipped { get; set; }
        public int Delivered { get; set; }
        public int Cancelled { get; set; }
    }

    public class ProductSection
    {
        public List<TopProductDto> TopSelling { get; set; }
        public List<LowStockProductDto> LowStock { get; set; }
    }

    public class CustomerSection
    {
        public int UniqueCustomers { get; set; }
        public int RepeatCustomers { get; set; }
    }

    public class PaymentSection
    {
        public decimal TotalRevenue { get; set; }
        public List<PaymentStatsDto> ByMethod { get; set; }
    }

    public class OtherMetricsSection
    {
        public decimal AverageOrderValue { get; set; }
        public int TotalReturns { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
    public class SalesOrdersDto
    {
        public decimal AllTimeSales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal TodaySales { get; set; }

        public int AllTimeOrders { get; set; }
        public int MonthlyOrders { get; set; }
        public int TodayOrders { get; set; }

        public OrderStatusBreakdownDto OrderStatuss { get; set; }
    }

    public class OrderStatusBreakdownDto
    {
        public int Pending { get; set; }
        public int Shipped { get; set; }
        public int Delivered { get; set; }
        public int Cancelled { get; set; }
    }

    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitsSold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class LowStockProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int AvailableQty { get; set; }
    }

    public class CustomerStatsDto
    {
        public int UniqueCustomers { get; set; }
        public int RepeatCustomers { get; set; }
    }

    public class PaymentStatsDto
    {
        public string PaymentMethod { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ReturnsStatsDto
    {
        public int TotalReturns { get; set; }
        public decimal RefundAmount { get; set; }
    }

    public class ReviewsStatsDto
    {
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }

}
