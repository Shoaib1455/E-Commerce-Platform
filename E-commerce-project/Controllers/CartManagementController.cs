using E_commerce.Models.Models;
using E_commerce.Repository.CartRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartManagementController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartManagementController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        [HttpPost("Add Product")]
        public async Task<bool> AddProductToCart([FromBody] CartdetailsVM cartdetails)
        {
            var userid= int.Parse( User.FindFirstValue(ClaimTypes.NameIdentifier));
            cartdetails.carts.Userid = userid;
            if (cartdetails == null)
            {
                return false;
            }
            var isproductadded = await _cartRepository.AddProductToCart(cartdetails);
            return isproductadded;
        }
        [HttpGet("User cart")]
        public async Task<CartDto> GetUsersCart(long userid)
        {
            var usercart = await _cartRepository.GetUserCart(userid);
            return usercart;

        }
        [HttpPost("Remove item")]
        public async Task<bool> RemoveItemFromCart(int itemid)
        {
            if (itemid == 0) { return false; }
            var isremoved = await _cartRepository.RemoveItemFromCart(itemid);
            return isremoved;
        }
        [HttpDelete("Empty Cart")]
        public async Task<bool> RemoveCart(int cartid)
        {
            var iscartremoved= await _cartRepository.EmptyCart(cartid);
            return iscartremoved;
        }
    }
}
