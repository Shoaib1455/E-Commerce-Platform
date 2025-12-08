using E_commerce.Models.Models;
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
        public PaymentController(IPaymentRepository paymentRepository, IConfiguration config)
        {
            _paymentRepository = paymentRepository;
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
            //try
            //{
            //    switch (stripeEvent.Type)
            //    {
            //        case Events.PaymentIntentSucceeded:
            //            await HandleSuccessfulPayment(stripeEvent);
            //            break;

            //        case Events.PaymentIntentPaymentFailed:
            //            await HandleFailedPayment(stripeEvent);
            //            break;

            //        case Events.PaymentIntentProcessing:
            //            Console.WriteLine("⌛ Payment processing...");
            //            break;

            //        default:
            //            Console.WriteLine($"ℹ️ Unhandled event type: {stripeEvent.Type}");
            //            break;
            //    }

            //    return Ok(); // MUST return 200 or Stripe retries
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("❌ Error processing webhook: " + ex.Message);
            //    return StatusCode(500);
            //}
            /*
            try
            {
                // 1. Read raw body (JSON)
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                var stripeSignatureHeader = Request.Headers["Stripe-Signature"];
                var webhookSecret = "whsec_Gt8OnKXuW9lsKZm9Z5MVd26VVMxT459R";   
                Event stripeEvent;
                try
                {
                    stripeEvent = EventUtility.ConstructEvent(
                        body,
                        stripeSignatureHeader,
                        webhookSecret
                    );
                }
                catch (StripeException e)
                {
                    // Signature verification failed
                    Console.WriteLine("⚠️ Webhook signature verification failed: " + e.Message);
                    return BadRequest();
                }
                // 2. Log or store for debugging
                Console.WriteLine("Webhook Received: " + body);

                // 3. Convert JSON to C# object (optional)
                var payload = JsonSerializer.Deserialize<PaymentEvent>(body);

                // 4. Handle the event
                await _paymentRepository.ProcessPaymentEvent(stripeEvent);

                // 5. Return 200 OK (most important)
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }*/
            //return Ok();
        }

        // public async Task<IActionResult> PaymentDetails(Payment)
        [HttpPost("createpaymentintent")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto)
        {
            //StripeConfiguration.ApiKey = "sk_test_51SXHeBINeRcPQQXNTL6LEkDIKmdZ9FvxgzJPf2ONX5Xy9M9xqJBLE1hjYpJQk3K5leTkqOf2lncZB0HgK4mX1Res00THdgn8kC"; // your secret key
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            var options = new PaymentIntentCreateOptions
            {
                Amount = dto.Amount, // in cents
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
