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
        public Task<CartdetailsVM> GetUserCart(long Userid);
        public Task<bool> UpdateQuantityOfItem(long itemid);
        public Task<bool> RemoveItemFromCart(long itemid);
        public Task<bool> RemoveCart(int cartid);
    }
}
