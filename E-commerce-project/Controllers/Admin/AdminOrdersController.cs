using E_commerce.Models.Models;
using E_commerce.Repository.OrderRepository;
using E_commerce.Repository.ProductRepository;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Admin
{
    public class AdminOrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public AdminOrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return orders;
        }

        public async Task<Order> GetOrdersById(long orderid)
        {
            var order = await _orderRepository.GetOrderById(orderid);
            return order;
        }
    }
}
