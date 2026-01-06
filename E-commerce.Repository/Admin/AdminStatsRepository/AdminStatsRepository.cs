using E_commerce.Models.Data;
using E_commerce.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.Admin.AdminStatsRepository
{
    public class AdminStatsRepository: IAdminStatsRepository
    {
        private readonly EcommerceContext _context;
        
        public AdminStatsRepository(EcommerceContext context) 
        {
            _context = context;
        }
        //public async Task<AdminDashboardOverviewDto> GetOverviewAsync()
        //{
        //    var today = DateTime.UtcNow.Date;

        //    var completedOrdersQuery = _context.Orders
        //        .AsNoTracking()
        //        .Where(o => o.Status == OrderStatus.Completed);

        //    var totalRevenue = await completedOrdersQuery.SumAsync(o => o.TotalAmount);
        //    var todayRevenue = await completedOrdersQuery
        //        .Where(o => o.CreatedAt >= today)
        //        .SumAsync(o => o.TotalAmount);

        //    var totalOrders = await _context.Orders.CountAsync();
        //    var todayOrders = await _context.Orders
        //        .CountAsync(o => o.Createdat >= today);

        //    var pendingOrders = await _context.Orders
        //        .CountAsync(o => o.Status == OrderStatus.Pending);

        //    var completedOrders = await completedOrdersQuery.CountAsync();

        //    var totalCustomers = await _context.Users
        //        .CountAsync(u => u.Role == UserRole.Customer);

        //    var averageOrderValue = completedOrders == 0
        //        ? 0
        //        : totalRevenue / completedOrders;

        //    return new AdminDashboardOverviewDto
        //    {
        //        TotalRevenue = totalRevenue,
        //        TodayRevenue = todayRevenue,
        //        TotalOrders = totalOrders,
        //        TodayOrders = todayOrders,
        //        PendingOrders = pendingOrders,
        //        CompletedOrders = completedOrders,
        //        TotalCustomers = totalCustomers,
        //        AverageOrderValue = Math.Round(averageOrderValue, 2)
        //    };
        //}
        //public async Task<List<AdminSalesTrendDto>> GetSalesTrendAsync(DateTime? startDate,DateTime? endDate,string groupBy)
        //{
        //    var query = _context.Orders
        //        .AsNoTracking()
        //        .Where(o =>
        //            o.Status == OrderStatus.Completed &&
        //            o.Createdat >= startDate &&
        //            o.Createdat <= endDate
        //        );

        //    if (groupBy.ToLower() == "month")
        //    {
        //        return await query
        //            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
        //            .Select(g => new AdminSalesTrendDto
        //            {
        //                Period = $"{g.Key.Year}-{g.Key.Month:D2}",
        //                TotalSales = g.Sum(x => x.TotalAmount),
        //                TotalOrders = g.Count()
        //            })
        //            .OrderBy(x => x.Period)
        //            .ToListAsync();
        //    }

        //    // Default: group by day
        //    return await query
        //        .GroupBy(o => o.CreatedAt.Date)
        //        .Select(g => new AdminSalesTrendDto
        //        {
        //            Period = g.Key.ToString("yyyy-MM-dd"),
        //            TotalSales = g.Sum(x => x.TotalAmount),
        //            TotalOrders = g.Count()
        //        })
        //        .OrderBy(x => x.Period)
        //        .ToListAsync();
        //}
        //public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int topN,DateTime? startDate = null,DateTime? endDate = null)
        //{
        //    var query = _context.Orderitems
        //        .AsNoTracking()
        //        .Include(oi => oi.Order)
        //        .Include(oi => oi.Product)
        //        .Where(oi => oi.Order.Status == OrderStatus.Completed);

        //    if (startDate.HasValue)
        //        query = query.Where(oi => oi.Order.CreatedAt >= startDate.Value);

        //    if (endDate.HasValue)
        //        query = query.Where(oi => oi.Order.CreatedAt <= endDate.Value);

        //    return await query
        //        .GroupBy(oi => new
        //        {
        //            oi.ProductId,
        //            oi.Product.Name
        //        })
        //        .Select(g => new TopSellingProductDto
        //        {
        //            ProductId = g.Key.ProductId,
        //            ProductName = g.Key.Name,
        //            TotalQuantitySold = g.Sum(x => x.Quantity),
        //            TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
        //        })
        //        .OrderByDescending(x => x.TotalRevenue)
        //        .Take(topN)
        //        .ToListAsync();
        //}
        //public async Task<CustomerStatsDto> GetCustomerStatsAsync(int topCustomers = 5)
        //{
        //    var today = DateTime.UtcNow.Date;
        //    var startOfMonth = new DateTime(today.Year, today.Month, 1);

        //    var customersQuery = _context.Usermanagements
        //        .AsNoTracking()
        //        .Where(u => u.Role == UserRole.Customer);// it is an enum

        //    var totalCustomers = await customersQuery.CountAsync();

        //    var newToday = await customersQuery
        //        .CountAsync(u => u.CreatedAt >= today);

        //    var newThisMonth = await customersQuery
        //        .CountAsync(u => u.CreatedAt >= startOfMonth);

        //    var returningCustomers = await _context.Orders
        //        .AsNoTracking()
        //        .Where(o => o.Status == OrderStatus.Completed)
        //        .Select(o => o.UserId)
        //        .Distinct()
        //        .CountAsync();

        //    var topCustomersData = await _context.Orders
        //        .AsNoTracking()
        //        .Where(o => o.Status == OrderStatus.Completed)
        //        .GroupBy(o => new { o.UserId, o.User.FullName })
        //        .Select(g => new TopCustomerDto
        //        {
        //            CustomerId = g.Key.UserId,
        //            CustomerName = g.Key.FullName,
        //            TotalOrders = g.Count(),
        //            TotalSpent = g.Sum(x => x.TotalAmount)
        //        })
        //        .OrderByDescending(x => x.TotalSpent)
        //        .Take(topCustomers)
        //        .ToListAsync();

        //    return new CustomerStatsDto
        //    {
        //        TotalCustomers = totalCustomers,
        //        NewCustomersToday = newToday,
        //        NewCustomersThisMonth = newThisMonth,
        //        ReturningCustomers = returningCustomers,
        //        TopCustomers = topCustomersData
        //    };
        //}
        //public async Task<List<LowStockProductDto>> GetLowStockProductsAsync()
        //{
        //    return await _context.Products
        //        .AsNoTracking()
        //        .Where(p => p.StockQuantity <= p.ReorderLevel)
        //        .Select(p => new LowStockProductDto
        //        {
        //            ProductId = p.Id,
        //            ProductName = p.Name,
        //            CurrentStock = p.StockQuantity,
        //            ReorderLevel = p.ReorderLevel
        //        })
        //        .OrderBy(p => p.CurrentStock)
        //        .ToListAsync();
        //}

        //public async Task<List<PaymentMethodStatsDto>> GetPaymentMethodStatsAsync( DateTime? startDate = null,DateTime? endDate = null)
        //{
        //    var query = _context.Orders
        //        .AsNoTracking()
        //        .Where(o => o.Status == OrderStatus.Completed);

        //    if (startDate.HasValue)
        //        query = query.Where(o => o.CreatedAt >= startDate.Value);

        //    if (endDate.HasValue)
        //        query = query.Where(o => o.CreatedAt <= endDate.Value);

        //    return await query
        //        .GroupBy(o => o.PaymentMethod)
        //        .Select(g => new PaymentMethodStatsDto
        //        {
        //            PaymentMethod = g.Key.ToString(),
        //            TotalOrders = g.Count(),
        //            TotalRevenue = g.Sum(x => x.TotalAmount)
        //        })
        //        .OrderByDescending(x => x.TotalRevenue)
        //        .ToListAsync();
        //}

        //public async Task<List<OrderStatusDto>> GetOrderStatusBreakdownAsync()
        //{
        //    return await _context.Orders
        //        .AsNoTracking()
        //        .GroupBy(o => o.Status)
        //        .Select(g => new OrderStatusDto
        //        {
        //            Status = g.Key.ToString(),
        //            TotalOrders = g.Count()
        //        })
        //        .OrderByDescending(x => x.TotalOrders)
        //        .ToListAsync();
        //}

    }
}
