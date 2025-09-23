using E_commerce.Models.Models;
using E_commerce.Repository.CartRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_commerce_project.Controllers
{
    public class CartManagementController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartManagementController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        [HttpPost]
        public async Task<bool> AddProductToCart([FromBody] CartdetailsVM cartdetails)
        {
            if (cartdetails == null)
            {
                return false;
            }
            var isproductadded = await _cartRepository.AddProductToCart(cartdetails);
            return isproductadded;
        }
        [HttpGet]
        public async Task<CartdetailsVM> GetUsersCart(long userid)
        {
            var usercart = await _cartRepository.GetUserCart(userid);
            return usercart;

        }
        [HttpPost]
        public async Task<bool> RemoveItemFromCart(long itemid)
        {
            if (itemid == 0) { return false; }
            var isremoved = await _cartRepository.RemoveItemFromCart(itemid);
            return isremoved;
        }
        [HttpGet]
        public async Task<bool> RemoveCart(int cartid)
        {
            var iscartremoved= await _cartRepository.RemoveCart(cartid);
            return iscartremoved;
        }
    }
}
