using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services
{
    public static class JWTService
    {
        private static IJsonSerializer _serializer = new JsonNetSerializer();
        private static IDateTimeProvider _provider = new UtcDateTimeProvider();
        private static IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
        private static IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

        public static DateTime GetExpiryTimestamp(string accessToken)
        {
            try
            {
                IJwtValidator _validator = new JwtValidator(_serializer, _provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
                var token = decoder.DecodeToObject<JToken>(accessToken);
                DateTimeOffset datetimeoffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(token["exp"].ToString()));

                return datetimeoffset.LocalDateTime;
            }
            catch (TokenExpiredException)
            {
                return DateTime.MinValue;
            }
            catch (SignatureVerificationException)
            {
                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                // ... remember to handle the generic exception ...
                return DateTime.MinValue;
            }
        }
        
            public static bool ValidateToken(string accessToken) {
            bool isValid = true;
            DateTime tokenExpiryTime = GetExpiryTimestamp(accessToken);
            if (DateTime.Now >= tokenExpiryTime)
            {
                isValid = false;
            }
            return isValid;
        }


    }
}
