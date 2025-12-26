using E_commerce.Repository.Admin.AdminStatsRepository;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Admin
{
    public class AdminDashboardController : Controller
    {
        private readonly IAdminStatsRepository _adminStatsRepository;

        public AdminDashboardController(IAdminStatsRepository adminStatsRepository)
        {
            _adminStatsRepository = adminStatsRepository;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var stats = await _adminStatsRepository.GetOverviewAsync();
            return Ok(stats);
        }
        [HttpGet("sales-trend")]
        public async Task<IActionResult> GetSalesTrend([FromQuery] DateTime startDate,[FromQuery] DateTime endDate,[FromQuery] string groupBy = "day")
        {
            var result = await _adminStatsRepository.GetSalesTrendAsync(startDate, endDate, groupBy);
            return Ok(result);
        }
        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int top = 10,[FromQuery] DateTime? startDate = null,[FromQuery] DateTime? endDate = null)
        {
            var result = await _adminStatsRepository.GetTopSellingProductsAsync(top, startDate, endDate);

            return Ok(result);
        }
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomerStats([FromQuery] int top = 5)
        {
            var result = await _adminStatsRepository.GetCustomerStatsAsync(top);
            return Ok(result);
        }

        //[HttpGet("low-stock")]
        //public async Task<IActionResult> GetLowStockProducts()
        //{
        //    var result = await _adminStatsRepository.GetLowStockProductsAsync();
        //    return Ok(result);
        //}

        //[HttpGet("payment-methods")]
        //public async Task<IActionResult> GetPaymentMethodStats([FromQuery] DateTime? startDate = null,[FromQuery] DateTime? endDate = null)
        //{
        //    var result = await _adminStatsRepository
        //        .GetPaymentMethodStatsAsync(startDate, endDate);

        //    return Ok(result);
        //}
        //[HttpGet("order-status-breakdown")]
        //public async Task<IActionResult> GetOrderStatusBreakdown()
        //{
        //    var result = await _adminStatsRepository.GetOrderStatusBreakdownAsync();
        //    return Ok(result);
        //}

    }
}
