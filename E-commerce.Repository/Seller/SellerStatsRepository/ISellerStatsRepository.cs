using E_commerce.ViewModels;
using E_commerce.ViewModels.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.Seller.SellerStatsRepository
{
    public interface ISellerStatsRepository
    {
        public Task<SellerDashboardOverviewDto> GetDashboardOverviewAsync(int sellerId);
        public Task<List<OrderitemDto>> GetSellerOrders(int sellerid);
        public Task<List<ProductVM>> GetSellerProducts(int sellerid);
    }
}
