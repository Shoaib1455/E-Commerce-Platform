using E_commerce.Models.Models;
using E_commerce.Repository.OrderRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderManagementController : Controller
    {
        private readonly IOrderRepository _orderrepository;
        public OrderManagementController(IOrderRepository orderrepository)
        {
            _orderrepository = orderrepository;
        }
        [HttpPost]
       public async Task<Order> checkoutuser(OrderRequestDto orderequest)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            foreach (var c in User.Claims)
            {
                Console.WriteLine($"{c.Type}: {c.Value}");
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result =await _orderrepository.Checkout(orderequest,userid,userEmail);
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
        public async Task<OrderDto> GetDetailsOfSingleOrder(int orderid)
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
