using E_commerce.Models.Models;
using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.CartRepository
{
    public interface ICartRepository
    {
        public Task<bool> AddProductToCart(CartdetailsVM cartdetails);
        public Task<CartDto> GetUserCart(long Userid);
        public Task<bool> UpdateQuantityOfItem(int itemid);
        public Task<bool> RemoveItemFromCart(int itemid);
        public Task<bool> EmptyCart(int cartid);
    }
}
