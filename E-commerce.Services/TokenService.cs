using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using E_commerce.Models.Models;

namespace E_commerce.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;

        }
        public string GenerateToken(Usermanagement user)
        {

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(ClaimTypes.Role,user.Role),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             
        };
            var keys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var cred = new SigningCredentials(keys, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["jwt:Issuer"],
                audience: _config["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cred
                );
            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

            return tokenhandler.WriteToken(token);
        }

        //public ClaimsPrincipal? ValidateToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

        //    try
        //    {
        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidIssuer = _config["Jwt:Issuer"],

        //            ValidateAudience = true,
        //            ValidAudience = _config["Jwt:Audience"],

        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),

        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero // remove 5 min default tolerance
        //        };

        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        //        // Extra check to ensure token uses the expected algorithm
        //        if (validatedToken is JwtSecurityToken jwtToken &&
        //            jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            return principal;
        //        }

        //        return null;
        //    }
        //    catch
        //    {
        //        return null; // invalid token
        //    }
        //}
    }
}