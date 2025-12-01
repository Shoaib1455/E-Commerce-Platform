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
        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        [HttpPost("payment")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentWebhook()
        {
            Request.EnableBuffering();
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var signatureHeader = Request.Headers["Stripe-Signature"];
            var webhookSecret = "whsec_Gt8OnKXuW9lsKZm9Z5MVd26VVMxT459R"; 

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
                return BadRequest("Signature verification failed");
            }

            Console.WriteLine("📥 Webhook received: " + stripeEvent.Type);
            await _paymentRepository.ProcessPaymentWebhookAsync(stripeEvent);
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
            return Ok();
        }

       // public async Task<IActionResult> PaymentDetails(Payment)
    }
}
