using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using E_commerce.ViewModels.Seller;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.Seller.SellerStatsRepository
{
    public class SellerStatsRepository:ISellerStatsRepository
    {
        private readonly EcommerceContext _context;
        public SellerStatsRepository(EcommerceContext context) 
        { 
        _context=context;
        }
        public async Task<SellerDashboardOverviewDto> GetDashboardOverviewAsync(int sellerId)
        {
            var today = DateTime.UtcNow.Date;
            var monthStart = new DateTime(today.Year, today.Month, 1);

            var orderList = await _context.Orderitems
                .AsNoTracking()
                .Where(o => o.Product.Sellerid == sellerId)
                .Select(oi => new
                {
                    oi.Orderid,
                    oi.Unitprice,
                    oi.Totalprice,
                    oi.Quantity,
                    oi.Order.Createdat,
                    oi.Order.Status
                })
    .ToListAsync();

            //var orderList = await orders.ToListAsync();

            /* ---------------- SALES & ORDERS ---------------- */

            var salesOrders = new SalesOrdersSection
            {
                AllTimeSales = orderList.Sum(o => (int)o.Totalprice),
                MonthlySales = orderList.Where(o => o.Createdat >= monthStart).Sum(o => (int)o.Totalprice),
                TodaySales = orderList.Where(o => o.Createdat >= today).Sum(o => (int)o.Totalprice),

                AllTimeOrders = orderList.Count,
                MonthlyOrders = orderList.Count(o => o.Createdat >= monthStart),
                TodayOrders = orderList.Count(o => o.Createdat >= today),

                OrderStatus = new OrderStatusBreakdown
                {
                    Pending = orderList.Count(o => o.Status == "Pending"),
                    Shipped = orderList.Count(o => o.Status == "Shipped"),
                    Delivered = orderList.Count(o => o.Status == "Delivered"),
                    Cancelled = orderList.Count(o => o.Status == "Cancelled")
                }
            };

            /* ---------------- PRODUCTS ---------------- */

            var topProducts = await _context.Orderitems
                .AsNoTracking()
                .Where(i => i.Product.Sellerid == sellerId)
                .GroupBy(i => new { i.Productid, i.Product.Name })
                .Select(g => new TopProductDto
                {
                    ProductId = (int)g.Key.Productid,
                    ProductName = g.Key.Name,
                    UnitsSold = g.Sum(x => (int)x.Quantity),
                    Revenue = g.Sum(x => (int)x.Totalprice * (int)x.Quantity)
                })
                .OrderByDescending(x => x.UnitsSold)
                .Take(5)
                .ToListAsync();

            var lowStock = await _context.Products
                .AsNoTracking()
                .Where(p => p.Sellerid == sellerId && p.Inventories.Any(i => i.Productid == p.Id && i.Quantityinstock <= 10))
                .Select(p => new LowStockProductDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    AvailableQty = p.Inventories
                                    .Where(i => i.Productid == p.Id)
                                    .Select(i => i.Quantityinstock)
                                    .FirstOrDefault()
                })
                .ToListAsync();

            /* ---------------- CUSTOMERS ---------------- */

            //var customerGroups = orderList.GroupBy(o => o.CustomerId);

            //var customers = new CustomerSection
            //{
            //    UniqueCustomers = customerGroups.Count(),
            //    RepeatCustomers = customerGroups.Count(g => g.Count() > 1)
            //};

            /* ---------------- PAYMENTS ---------------- */

            var payments = new PaymentSection
            {
                TotalRevenue = orderList.Sum(o => (int)o.Totalprice) //.ToList()
                //ByMethod = orderList
                //    .GroupBy(o => o.Order.PaymentMethod)
                //    .Select(g => new PaymentStatsDto
                //    {
                //        PaymentMethod = g.Key,
                //        Orders = g.Count(),
                //        Revenue = g.Sum(x => x.TotalAmount)
                //    })
                    
            };

            ///* ---------------- OTHER METRICS ---------------- */

            //var totalReturns = await _context.Returns
            //    .AsNoTracking()
            //    .Where(r => r.SellerId == sellerId)
            //    .ToListAsync();

            //var reviews = await _db.Reviews
            //    .AsNoTracking()
            //    .Where(r => r.SellerId == sellerId)
            //    .ToListAsync();

            //var otherMetrics = new OtherMetricsSection
            //{
            //    AverageOrderValue = orderList.Any()
            //        ? orderList.Average(o => o.TotalAmount)
            //        : 0,

            //    TotalReturns = totalReturns.Count,
            //    RefundAmount = totalReturns.Sum(r => r.RefundAmount),

            //    AverageRating = reviews.Any()
            //        ? reviews.Average(r => r.Rating)
            //        : 0,

            //    TotalReviews = reviews.Count
            //};

            /* ---------------- FINAL RESPONSE ---------------- */

            return new SellerDashboardOverviewDto
            {
                SalesOrders = salesOrders,
                Products = new ProductSection
                {
                    TopSelling = topProducts,
                    LowStock = lowStock
                },
                //Customers = customers,
                Payments = payments,
                //OtherMetrics = otherMetrics
            };
        }

        public async Task<List<OrderitemDto>> GetSellerOrders(int sellerid)
        {
            var orders = await _context.Orderitems
                .Where(oi => oi.Product.Sellerid == sellerid)
                .Select(ol => new OrderitemDto
                {
                    Productid = (int)ol.Productid,
                    Quantity = (int)ol.Quantity,
                    Unitprice = (int)ol.Unitprice,
                    Totalprice = (int)ol.Totalprice,
                    Orderid = (int)ol.Orderid,
                    Status = ol.Order.Status,
                    Createdat = (DateTime)ol.Order.Createdat,
                    Customername=ol.Order.User.Name
                })
                .ToListAsync();
            return orders;
            
        }
        public async Task<List<ProductVM>> GetSellerProducts(int sellerid)
        {
            var products = await _context.Products
                .Where(p=> p.Sellerid == sellerid)
                .Select(p=> new ProductVM
                {
                    Id = p.Id,
                    Name=p.Name,
                    Sku=p.Sku,
                    CategoryName=p.Category.Name,
                    Quantity =p.Inventories.Where(i => (int)i.Productid == (int)p.Id).First().Quantityinstock,
                    Price = p.Price,
                    Status=(p.Isactive==true)?"Active":"Not Active",
                })
                .ToListAsync();
            return products;

        }
    }
}
