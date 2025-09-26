using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

                foreach (var item in cartdetails.cartitems)
                {
                    Cartitem citems = new Cartitem()
                    {
                        Cartid = newcartid,
                        Productid = item.Productid,
                        Quantity =  item.Quantity,
                        Unitprice = item.Unitprice,

                    };
                    await _context.Cartitems.AddAsync(citems);
                    await _context.SaveChangesAsync();
                }
             }
            else
            {
                cartdetails1.Isactive= true;
                foreach (var item in cartdetails.cartitems)
                {
                    var cartitemdetails = _context.Cartitems.Where(c => c.Cartid == cartdetails1.Id && c.Productid == item.Productid).FirstOrDefault();
                    if (cartitemdetails == null)
                    {
                        Cartitem crtitems = new Cartitem()
                        {
                            Cartid = cartdetails1.Id,
                            Productid = item.Productid,
                            Quantity =  item.Quantity,
                            Unitprice = item.Unitprice,

                        };
                        await _context.Cartitems.AddAsync(crtitems);
                    }
                    else
                    {
                        cartitemdetails.Quantity += item.Quantity;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<CartDto> GetUserCart(long Userid)
        {
            var usercart = await _context.Carts.Include(c => c.Cartitems).Where(c => c.Userid == Userid && c.Isactive==true).FirstOrDefaultAsync();
            if (usercart == null) 
            {
                return new CartDto();
            }
            CartDto cart = new CartDto()
            {
                Userid= usercart.Userid,
                Status= usercart.Status,
                Isactive= usercart.Isactive,
                Cartitems= usercart.Cartitems.Select(ci=>  new CartitemDto
                {
                    Cartid = ci.Cartid,
                    Productid= ci.Productid,
                    Quantity= ci.Quantity,
                    Unitprice= ci.Unitprice,
                }).ToList(),
            };

            return cart;
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
            
             var cartitems = await _context.Cartitems.Where(c => c.Cartid == id).ToListAsync();
            if (cartitems.Count == 0)
            {
                var cart= await _context.Carts.FindAsync(id);
                cart.Isactive = false;
                await _context.SaveChangesAsync();
            }

            return true;
        }
        public async Task<bool> EmptyCart(int cartid)
        {
            var cartitems = await _context.Cartitems.Where(c=>c.Cartid==cartid).ToListAsync();
            var cart=await _context.Carts.Where(c=> c.Id== cartid).FirstOrDefaultAsync();
            if (cartitems.Any())
            {
                _context.Cartitems.RemoveRange(cartitems);
                
            }
            cart.Isactive = false;

            //_context.Carts.Remove(cart);

            await _context.SaveChangesAsync();
            return true;
        }
        //private async Task<Cart> CreateCart(Cart)
        //{

        //}

    }
}
