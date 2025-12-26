using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.Services.EmailService;
using E_commerce.Services.NotificationService;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.OrderRepository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly EcommerceContext _context;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        public OrderRepository(EcommerceContext context, INotificationService notificationService, IEmailService emailService) 
        { 
            _context = context;
            _notificationService= notificationService;
            _emailService = emailService;
        }
        public async Task<Order> Checkout(OrderRequestDto payload,int userid,string email)
        {
            //var cart = await _context.Carts.Where(u => u.Userid == userid && u.Isactive==true).FirstOrDefaultAsync();
            //var cartitems = await _context.Cartitems.Where(u => u.Cartid == cart.Id).ToListAsync();
            //
            //if (cart == null)
            //{
            //    return null;
            //}

            var address = _context.Addresses.Where(o => o.Userid == userid && o.Isdefault == true).FirstOrDefault();
            Address newAddress = new Address()
            {
                //FullName = payload.Address.Shipping.FullName,
                Phone = (int)payload.Address.Shipping.Phone,
                Street = payload.Address.Shipping.Street,
                City = payload.Address.Shipping.City,
                Postalcode = payload.Address.Shipping.PostalCode,
                Country = payload.Address.Shipping.Country,
                State = payload.Address.Shipping.State,
                Isdefault = (address == null) ? true : false,
                Userid = userid
            };
            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();


            if (address == null && newAddress == null)
            {
                throw new Exception("No default address found. Please add or select an address before checkout.");
            }
            //else if (address == null)
            //{
            //    order.Addressid = newAddress.Id;
            //}
            //else
            //{
            //    order.Addressid = address.Id;
            //}
            await _context.SaveChangesAsync();
            Order order = new Order()
            {
                Userid = userid,
                TotalAmount = payload.Order.TotalAmount,
                Shippingfee=payload.Order.ShippingFee,
                Paymentmethod=payload.Order.PaymentMethod,
                Status ="pending",
                Addressid=newAddress.Id
                
            };
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderids = order.Id;
            var orders= _context.Orders.Where(o=>o.Userid==userid && o.Id== orderids).FirstOrDefault();
            
            foreach (var item in payload.OrderItems)
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
            //cart.Isactive = false;
            //_context.Cartitems.RemoveRange(cartitems);
            await _context.SaveChangesAsync();
            //if (orders != null)
            //{
            //    var result =await _context.Orderitems.Where(o => o.Orderid == orders.Id).SumAsync(o => o.Totalprice);
            //    orders.TotalAmount = result;
            //    await _context.SaveChangesAsync();
            //}
            await _notificationService.SendToUserAsync(
            userid,
            new NotificationDto
            {
                Title = "Order Placed",
                Message = $"Your order #{orders.Id} has been placed successfully.",
                OrderId = orders.Id.ToString()
            }
        );
            await _emailService.SendOrderConfirmationEmailAsync(email, orders);
            return orders;
        }
        public async Task<List<Order>> GetAllOrdersForSingleUser(int userid)
        {
            var orders=await _context.Orders.Where(o=>o.Userid == userid).ToListAsync();
            return orders;
        }
        public async Task<OrderDto> GetDetailsOfSingleOrder(int orderid)
        {
           // var order = await _context.Orders.Where(o => o.Id == orderid).FirstAsync();
            var order = await _context.Orders.Include(o=>o.Orderitems).Where(o => o.Id == orderid).FirstOrDefaultAsync();
            if (order != null)
            {
                OrderDto orderdt = new OrderDto()
                {
                    Userid = order.Userid,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Orderitem = order.Orderitems.Select(oi => new OrderitemDto
                    {
                        Id = oi.Id,
                        Productid = oi.Productid,
                        Quantity = oi.Quantity,
                        Unitprice = oi.Unitprice,
                        Totalprice = oi.Totalprice,
                        Orderid = oi.Orderid
                    }).ToList(),
                };
                return orderdt;
            }
            return new OrderDto();
            

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

        public async Task<Order> GetOrderById(long orderid)
        {
            var order =await _context.Orders.Where(o=> o.Id==orderid).FirstOrDefaultAsync();

            return order; 
        }


    }
}
