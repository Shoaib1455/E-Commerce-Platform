
using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using static E_commerce.ViewModels.PaymentDto;

namespace E_commerce.Repository.PaymentRepository
{
   

    public class PaymentRepository:IPaymentRepository
    {
        private readonly EcommerceContext _context;
        public PaymentRepository(EcommerceContext context)
        {
            _context = context;
        }
        //public async Task ProcessPaymentEvent(Event stripeEvent )
        //{
        //    if (stripeEvent.Type == "payment_intent.succeeded")
        //    {
        //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //        var orderId = paymentIntent.Metadata.ContainsKey("OrderId")
        //            ? paymentIntent.Metadata["OrderId"]
        //            : "Unknown";

        //        Console.WriteLine("Payment Success for Order: " + orderId);
        //        //await MarkOrderPaid(orderId);
        //    }
        //    else if (stripeEvent.Type == "payment_intent.payment_failed")
        //    {
        //        var failedIntent = stripeEvent.Data.Object as PaymentIntent;
        //        var failedOrderId = failedIntent.Metadata.ContainsKey("OrderId")
        //            ? failedIntent.Metadata["OrderId"]
        //            : "Unknown";

        //        Console.WriteLine("Payment Failed for Order: " + failedOrderId);
        //        //await HandleFailedPayment(failedOrderId);
        //    }
        //    else if (stripeEvent.Type == "charge.refunded")
        //    {
        //        var refundedCharge = stripeEvent.Data.Object as Charge;
        //        Console.WriteLine("Charge Refunded: " + refundedCharge.Id);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Unhandled event type: " + stripeEvent.Type);
        //    }
        //}
        public async Task ProcessPaymentEvent(Event stripeEvent)
        {
            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                var paymentIntentId = session.PaymentIntentId;

                // Retrieve the payment intent to confirm status and get amount
                var service = new PaymentIntentService();
                var paymentIntent = service.Get(paymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    var orderId = paymentIntent.Metadata.ContainsKey("OrderId")
                        ? paymentIntent.Metadata["OrderId"]
                        : null;

                    if (orderId != null)
                    {
                        // Save transaction info in Payment table
                        //var payment = new Payment
                        //{
                        //    OrderId = int.Parse(orderId),
                        //    Transactionid = (int)paymentIntentId,
                        //    Amount = paymentIntent.AmountReceived / 100m, // Stripe amount in cents
                        //    Status = paymentIntent.Status,
                        //    CreatedAt = DateTime.UtcNow
                        //};

                        //_context.Payments.Add(payment);

                        // Mark order as paid
                        var order = _context.Orders.FirstOrDefault(o => o.Id == int.Parse(orderId));
                        if (order != null)
                        {
                            order.Status = "Paid";
                        }

                        await _context.SaveChangesAsync();

                        Console.WriteLine("Order marked as paid: " + orderId);
                    }
                }
            }
            else if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                var orderId = paymentIntent.Metadata.ContainsKey("OrderId")
                    ? paymentIntent.Metadata["OrderId"]
                    : null;

                if (orderId != null)
                {
                    var payment = new Payment
                    {
                        Transactionid =  paymentIntent.Id,
                        Amount = paymentIntent.AmountReceived,
                        Status = paymentIntent.Status,
                        Orderid = int.Parse(orderId),
                    };

                    _context.Payments.Add(payment);

                    var order = _context.Orders.FirstOrDefault(o => o.Id == int.Parse(orderId));
                    if (order != null)
                    {
                        order.Status = "Paid";
                    }

                    await _context.SaveChangesAsync();
                    Console.WriteLine("Order marked as paid: " + orderId);
                }
            }
            else
            {
                Console.WriteLine("Unhandled event type: " + stripeEvent.Type);
            }
        }
        public async Task ProcessPaymentWebhookAsync(Event stripeEvent)
        {
            try
            {
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        await HandleSuccessfulPayment(stripeEvent);
                        break;

                    case "payment_intent.payment_failed":
                        await HandleFailedPayment(stripeEvent);
                        break;

                    default:
                        Console.WriteLine("Unhandled event: " + stripeEvent.Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Payment Processing Error: " + ex.Message);
                throw;
            }
        }
        private async Task HandleSuccessfulPayment(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            //var orderId = paymentIntent.Metadata["order_id"];
            var transactionId = paymentIntent.Id;
            var amount = paymentIntent.AmountReceived;

            //Console.WriteLine($"✅ Payment Succeeded for Order: {orderId}");

            await UpdateOrderPaymentAsync(new PaymentUpdateDto
            {
                //OrderId = int.Parse(orderId),
                TransactionId = transactionId,
                Amount = amount,
                Status = "Succeeded",
                //PaymentMethod = paymentIntent.PaymentMethod,
                PaymentDate = DateTime.UtcNow
            });
        }

        private async Task HandleFailedPayment(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            Console.WriteLine($"❌ Payment FAILED for Order: {paymentIntent.Metadata["order_id"]}");

            await UpdateOrderPaymentAsync(new PaymentUpdateDto
            {
                OrderId = int.Parse(paymentIntent.Metadata["order_id"]),
                TransactionId = paymentIntent.Id,
                Amount = paymentIntent.Amount,
                Status = "Failed",
                PaymentDate = DateTime.UtcNow
            });
        }
        public async Task UpdateOrderPaymentAsync(PaymentUpdateDto dto)
        {
            var order = await _context.Orders.FindAsync(dto.OrderId);

            if (order == null)
                throw new Exception("Order not found");

            // Update payment table
            var payment = new Payment
            {
                Orderid = dto.OrderId,
                Transactionid = dto.TransactionId,
                Amount = dto.Amount,
                Status = dto.Status,
               // PaymentMethod = dto.PaymentMethod,
               // PaymentDate = dto.PaymentDate
            };

            _context.Payments.Add(payment);

            // Update order status
            order.Status = dto.Status; // Succeeded / Failed
           // order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}


