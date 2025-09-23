using E_commerce.Models.Data;
using E_commerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.OrderRepository
{
    public class OrderRepository
    {
        private readonly EcommerceContext _context;
        public OrderRepository(EcommerceContext context) 
        { 
            _context = context;
        }
        public async Task<Order> Checkout(int userid)
        {
            var cart = await _context.Carts.Where(u => u.Userid == userid).FirstOrDefaultAsync();
            var cartitems = await _context.Cartitems.Where(u => u.Cartid == cart.Id).ToListAsync();
            if (cart == null)
            {
                return null;
            }
            Order order = new Order()
            {
                Userid = userid,
                TotalAmount = 0
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var orders= _context.Orders.Where(o=>o.Userid==userid).FirstOrDefault();
            foreach (var item in cartitems)
            {
                Orderitem orderitem = new Orderitem()
                {
                    Productid=item.Productid,
                    Quantity = item.Quantity,
                    Unitprice = item.Unitprice,
                    Totalprice=item.Unitprice * item.Quantity,
                    Orderid= orders.Id,
                };
                _context.Orderitems.Add(orderitem);
            }
            await _context.SaveChangesAsync();
            if (orders != null)
            {
                var result =await _context.Orderitems.Where(o => o.Orderid == orders.Id).SumAsync(o => o.Totalprice);
                orders.TotalAmount = result;
                await _context.SaveChangesAsync();
            }

            return orders;
        }
        public async Task<List<Order>> GetAllOrdersForSingleUser(int userid)
        {
            var orders=await _context.Orders.Where(o=>o.Id==userid).ToListAsync();
            return orders;
        }
        public async Task<Order> GetDetailsOfSingleOrder(int orderid)
        {
           // var order = await _context.Orders.Where(o => o.Id == orderid).FirstAsync();
            var order = await _context.Orders.Where(o => o.Id == orderid).FirstOrDefaultAsync();
            
            return order;

        }
        public async Task<Order> CancelOrder(int orderid)
        {
            var order=await _context.Orders.Where(o => o.Id == orderid).FirstOrDefaultAsync();
            if (order.Status.ToLower() == "shipped")
            {
                return order;
            }
            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return order;
        }
        //admin methods
        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }
        public async Task<Order> UpdateOrderStatus(int orderid,string status)
        {
            var order = await _context.Orders.Where(o=> o.Id==orderid).FirstOrDefaultAsync();
            order.Status=status;
            await _context.SaveChangesAsync();

            return order;
        }


    }
}
