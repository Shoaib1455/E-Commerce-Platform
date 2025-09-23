using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.CartRepository
{
    public class CartRepository
    {
        private readonly EcommerceContext _context;
        public CartRepository(EcommerceContext context) 
        {
             _context = context;
        }
        public async Task<bool> AddProductToCart(Cart cart,Cartitem cartitem)
        {
            var cartdetails=await _context.Carts.Where(c=>c.Id==cart.Id).FirstOrDefaultAsync();
       
            if (cartdetails == null)
            {
                Cart c = new Cart()
                {
                    Userid = cart.Userid,
                    Status = cart.Status

                };
                 await _context.Carts.AddAsync(c);
                var newcart = await _context.Carts.Where(c => c.Id == cart.Id).FirstOrDefaultAsync();
                Cartitem citems = new Cartitem()
                {
                    Cartid= newcart.Id,
                    Productid= cartitem.Productid,
                    Quantity= cartitem.Quantity,
                    Unitprice=cartitem.Unitprice,

                };
            }
            var cartitemdetails = _context.Cartitems.Where(c => c.Cartid == cartdetails.Id && c.Productid== cartitem.Productid).FirstOrDefault();
            if (cartitemdetails==null)
            {
                Cartitem crtitems = new Cartitem()
                {
                    Cartid = cartdetails.Id,
                    Productid = cartitem.Productid,
                    Quantity = cartitem.Quantity,
                    Unitprice = cartitem.Unitprice,

                };
            }
            else
            {
                cartitemdetails.Quantity += cartitem.Quantity;
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CartdetailsVM> GetUserCart(long Userid)
        {
            var usercart = _context.Carts.Include(c => c.Cartitems).FirstOrDefault(c => c.Userid == Userid && c.Status == "Active");
            if (usercart == null) 
            {

            }
            CartdetailsVM cartdetailsVM = new CartdetailsVM()
            {
                carts = usercart,
                cartitems=usercart.Cartitems.ToList(),
            };

            return cartdetailsVM;
        }
        public async Task<bool> UpdateQuantityOfItem(long itemid)
        {
            var cartitem = await _context.Cartitems.FindAsync(itemid);
            if (cartitem != null)
            {
                cartitem.Quantity += 1;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveItemFromCart(long itemid)
        {
            var cartitem = await _context.Cartitems.FindAsync(itemid);
            if (cartitem != null)
            {
               _context.Cartitems.Remove(cartitem);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveCart(int cartid)
        {
            var cartitems = await _context.Cartitems.Where(c=>c.Cartid==cartid).ToListAsync();
            var cart=await _context.Carts.Where(c=> c.Id== cartid).FirstOrDefaultAsync();
            if (cartitems.Any())
            {
                _context.Cartitems.RemoveRange(cartitems);
                
            }
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();
            return true;
        }
        //private async Task<Cart> CreateCart(Cart)
        //{

        //}

    }
}
