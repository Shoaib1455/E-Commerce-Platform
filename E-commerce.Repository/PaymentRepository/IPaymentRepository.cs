using E_commerce.Models.Models;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static E_commerce.ViewModels.PaymentDto;

namespace E_commerce.Repository.PaymentRepository
{
    public interface IPaymentRepository
    {
        //public Task ProcessPaymentEvent(Event stripeEvent );
        public Task<Payment> ProcessPaymentWebhookAsync(Event stripeEvent);
    }
}
