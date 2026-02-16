using E_commerce.Models.Models;
using E_commerce.Repository.OrderRepository;
using E_commerce.Repository.PaymentRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

using System.Text.Json;
using static E_commerce.ViewModels.PaymentDto;
namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        public PaymentController(IPaymentRepository paymentRepository, IConfiguration config, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _config = config;
        }
        [HttpPost("payment")]
        [AllowAnonymous]
        public async Task<Payment> PaymentWebhook()
        {
            Request.EnableBuffering();
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var signatureHeader = Request.Headers["Stripe-Signature"];
            var webhookSecret = _config["Stripe:WebhookSecret"];//"whsec_Gt8OnKXuW9lsKZm9Z5MVd26VVMxT459R"; 

            Event stripeEvent; 

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    webhookSecret
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Webhook signature verification FAILED: " + ex.Message);
                return new Payment();
            }

            Console.WriteLine("📥 Webhook received: " + stripeEvent.Type);
            return await _paymentRepository.ProcessPaymentWebhookAsync(stripeEvent);
            
        }

        // public async Task<IActionResult> PaymentDetails(Payment)
        [HttpPost("createpaymentintent")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto)
        {
            //StripeConfiguration.ApiKey = "sk_test_51SXHeBINeRcPQQXNTL6LEkDIKmdZ9FvxgzJPf2ONX5Xy9M9xqJBLE1hjYpJQk3K5leTkqOf2lncZB0HgK4mX1Res00THdgn8kC"; // your secret key
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            var order = await _orderRepository.GetOrderById(dto.OrderId);
            var amountinusd = _paymentRepository.ConvertPkrToUsd((int) order.TotalAmount, 279.5);
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)Math.Round(amountinusd * 100), // in cents
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                },
                Metadata = new Dictionary<string, string>
    {
        { "orderId", dto.OrderId.ToString() }  // attach your order ID here
    }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return Ok(new { clientSecret = intent.ClientSecret });
        }

    }
}
