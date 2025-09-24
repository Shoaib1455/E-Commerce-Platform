using E_commerce.Models.Models;
using E_commerce.Repository.OrderRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_project.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly IOrderRepository _orderrepository;
        public OrderManagementController(IOrderRepository orderrepository)
        {
            _orderrepository = orderrepository;
        }
        [HttpPost("checkout")]
       public async Task<Order> checkoutuser()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result =await _orderrepository.Checkout(userid);
            return result;
        }
        [HttpGet("orders of single user")]
        public async Task<List<Order>> GetAllOrderOfSingleUser()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders= await _orderrepository.GetAllOrdersForSingleUser(userid);
            return orders;
        }
        [HttpGet("details of single order")]
        public async Task<Order> GetDetailsOfSingleOrder(int orderid)
        {
            var singleorder = await _orderrepository.GetDetailsOfSingleOrder(orderid);
            return singleorder;
        }
        [HttpGet("cancel order")]
        public async Task<Order> CancelOrder(int orderid)
        {
            var cancelorder = await _orderrepository.CancelOrder(orderid);
            return cancelorder;
        }

    }
}
