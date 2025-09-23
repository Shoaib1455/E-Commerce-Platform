using E_commerce.Models.Data;
using E_commerce.Models.Models;

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

            var ignoredPaths = new[] { "/api/UserManagement/Login", "/api/UserManagement/Register", "/api/UserManagement/forgot-password", "/api/UserManagement/reset-password"};
            if (ignoredPaths.Contains(context.Request.Path.Value, StringComparer.OrdinalIgnoreCase))
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
