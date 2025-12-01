using E_commerce.Models.Data;
using E_commerce.Models.Models;
using System;
using System.IO;

namespace E_commerce_project.Middleware
{
   public class TokenValidationMiddleware
   {
        private readonly RequestDelegate _next;
        //private readonly EcommerceContext _db;
        public TokenValidationMiddleware(RequestDelegate next) //EcommerceContext db)
        {
            _next = next;
            //_db = db;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            

            var ignoredPaths = new[] { "/api/UserManagement/Login", "/api/UserManagement/Register", "/api/UserManagement/forgot-password", "/api/UserManagement/reset-password","/swagger",
        "/swagger/index.html","/api/ProductManagement/GetAllProducts","/api/AdminUsers/CreateUser","/api/SellerUsers/CreateUser","/api/webhooks/payment","/api/products/{id}"};
            var path = context.Request.Path.Value;
            if (ignoredPaths.Contains(context.Request.Path.Value, StringComparer.OrdinalIgnoreCase)|| path.StartsWith("/api/products", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // Skip middleware logic
                return;
            }
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            await _next(context); 
            return;

        }
    }
}
