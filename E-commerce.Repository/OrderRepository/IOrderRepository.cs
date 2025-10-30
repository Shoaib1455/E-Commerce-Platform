using E_commerce.Models.Models;
using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.OrderRepository
{
    public interface IOrderRepository
    {
        public Task<Order> Checkout(int userid);
        public Task<List<Order>> GetAllOrdersForSingleUser(int userid);
        public  Task<OrderDto> GetDetailsOfSingleOrder(int orderid);
        public Task<Order> CancelOrder(int orderid);
        public Task<List<Order>> GetAllOrders();
        public Task<Order> UpdateOrderStatus(int orderid, string status);
        public Task<Order> GetOrderById(long orderid);
    }
}
