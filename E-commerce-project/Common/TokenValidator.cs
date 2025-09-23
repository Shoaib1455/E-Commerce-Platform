using E_commerce.Services;

namespace E_commerce_project.Common
{
    public static class TokenValidator
    {
        public static bool isValidToken(HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            { 
                var token =authHeader.Substring("Bearer ".Length).Trim();
                return JWTService.ValidateToken(token);
            }
            return false;
        }
    }
}
