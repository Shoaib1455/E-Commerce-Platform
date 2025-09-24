using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.CartRepository
{
    public class CartRepository:ICartRepository
    {
        private readonly EcommerceContext _context;
        public CartRepository(EcommerceContext context) 
        {
             _context = context;
        }
        //Cart cart,Cartitem cartitem
        public async Task<bool> AddProductToCart(CartdetailsVM cartdetails)
        {
            var cartdetails1=await _context.Carts.FirstOrDefaultAsync(c => c.Userid == cartdetails.carts.Userid);

            if (cartdetails1 == null)
            {
                Cart c = new Cart()
                {
                    Userid = cartdetails.carts.Userid,
                    Status = cartdetails.carts.Status

                };
                await _context.Carts.AddAsync(c);
                await _context.SaveChangesAsync();
                var newcartid = c.Id;
                Cartitem citems = new Cartitem()
                {
                    Cartid = newcartid,
                    Productid = cartdetails.cartitems[0].Productid,
                    Quantity = cartdetails.cartitems[0].Quantity,
                    Unitprice = cartdetails.cartitems[0].Unitprice,

                };
                await _context.Cartitems.AddAsync(citems);
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartitemdetails = _context.Cartitems.Where(c => c.Cartid == cartdetails1.Id && c.Productid == cartdetails.cartitems[0].Productid).FirstOrDefault();
                if (cartitemdetails == null)
                {
                    Cartitem crtitems = new Cartitem()
                    {
                        Cartid = cartdetails1.Id,
                        Productid = cartdetails.cartitems[0].Productid,
                        Quantity = cartdetails.cartitems[0].Quantity,
                        Unitprice = cartdetails.cartitems[0].Unitprice,

                    };
                    await _context.Cartitems.AddAsync(crtitems);
                }
                else
                {
                    cartitemdetails.Quantity += cartdetails.cartitems[0].Quantity;
                }
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<CartListVM> GetUserCart(long Userid)
        {//&& c.Status == "Active"is active flag will be added
            var usercart = _context.Carts.Include(c => c.Cartitems).Where(c => c.Userid == Userid).ToList();
            if (usercart == null) 
            {

            }
            CartListVM cartdetailsVM = new CartListVM()
            {
                Cart = usercart.ToList()
                
            };

            return cartdetailsVM;
        }
        public async Task<bool> UpdateQuantityOfItem(int itemid)
        {
            var cartitem = await _context.Cartitems.FindAsync(itemid);
            if (cartitem != null)
            {
                cartitem.Quantity += 1;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveItemFromCart(int itemid)
        {
            var cartitem = await _context.Cartitems.FindAsync(itemid);
            int? id = 0;
            if (cartitem != null)
            {
                 id = cartitem.Cartid;
                _context.Cartitems.Remove(cartitem);
                await _context.SaveChangesAsync();
            }
            var cartitems = await _context.Cartitems.ToListAsync();
            if (cartitems == null)
            {
                var cart= await _context.Carts.FindAsync(id);
               // cart.Status=
            }
            var cartdetails = await _context.Cartitems.FindAsync(id);
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
