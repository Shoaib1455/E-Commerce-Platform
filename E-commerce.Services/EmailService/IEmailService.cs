using E_commerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.EmailService
{
    public interface IEmailService
    {

       public Task SendOrderConfirmationEmailAsync(string toEmail, Order order);
    }
}
