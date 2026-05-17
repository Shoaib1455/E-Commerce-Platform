using E_commerce.Repository.Seller.SellerStatsRepository;
using E_commerce.ViewModels;
using E_commerce.ViewModels.Seller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_commerce_project.Controllers.Seller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    
    public class SellerDashboardController : Controller
    {
        private readonly ISellerStatsRepository _sellerDashboardRepository;
        public SellerDashboardController(ISellerStatsRepository sellerDashboardRepository)
        {
            _sellerDashboardRepository= sellerDashboardRepository;
        }
        [HttpGet]
        public async Task<SellerDashboardOverviewDto> DashboardData()
        {
            var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var dashboardData = await _sellerDashboardRepository.GetDashboardOverviewAsync(sellerId);
            return dashboardData;
        }
        [HttpGet]
        public async Task<List<OrderitemDto>> GetSellerOrders()
        {
            var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var sellerOrdersData = await _sellerDashboardRepository.GetSellerOrders(sellerId);
            return sellerOrdersData;
        }
        [HttpGet]
        public async Task<List<ProductVM>> GetSellerProducts()
        {
            var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var sellerProductsData = await _sellerDashboardRepository.GetSellerProducts(sellerId);
            return sellerProductsData;
        }


    }
}
