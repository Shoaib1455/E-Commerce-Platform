using E_commerce.Models.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using MimeKit;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace E_commerce.Services.EmailService
{
    public class EmailService:IEmailService
    {
       private readonly IConfiguration _config;
        public EmailService(IConfiguration config) 
        {
            _config = config;
        }
        public async Task SendOrderConfirmationEmailAsync(string toEmail, Order order) 
        {
            var template = LoadTemplate("OrderConfirmation.html");

            template = template.Replace("{{OrderId}}", order.Id.ToString())
                               .Replace("{{TotalAmount}}", order.TotalAmount.ToString())
                               .Replace("{{OrderDate}}", DateTime.Now.ToString("dd MMM yyyy"));

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("E-Commerce Store", _config["EmailSettings:From"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = $"Order Confirmation - Order #{order.Id}";

            email.Body = new BodyBuilder { HtmlBody = template }.ToMessageBody();

            using var smtp = new SmtpClient();
             smtp.Connect(_config["EmailSettings:SmtpHost"], int.Parse(_config["EmailSettings:Port"]), false);
            await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        private string LoadTemplate(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                                    "..", "..", "..", "..", 
                                    "E-commerce.Services",
                                    "EmailService",
                                    "Templates",fileName
                                    );
            var relativePath = @"C:\Users\Muhammad Shoaib\source\repos\E-commerce-project\E-commerce.Services\EmailService\Templates\OrderConfirmation.html";
            
            //"E-commerce.Services",
            //                        "EmailService",
            //                        "Templates"fileName
            return File.ReadAllText(relativePath);
        }
    }
}
