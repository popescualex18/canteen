using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SCNeagtovo.Api.Authorization.Helpers
{
    public interface ITokenHelper
    {

        public ClaimsPrincipal GetPrincipalFromToken(string token, string secret, bool isRefreshCall = false);
        IList<Claim> GetClaims(string token);
    }

    public class TokenHelper : ITokenHelper
    {
        private (ClaimsPrincipal, SecurityToken) ValidateToken(string token, string secret, bool validateLifetime = true)
        {
            IdentityModelEventSource.ShowPII = true;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                //Same Secret key will be used while creating the token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };

            var principal =
                new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var securityToken);

            return (principal, securityToken);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, string secret, bool isRefreshCall = false)
        {
            var (principal, _) = ValidateToken(token, secret, !isRefreshCall);

            return principal;
        }

        public bool IsTokenExpired(string token, string secret)
        {
            try
            {
                GetPrincipalFromToken(token, secret);
            }
            catch (SecurityTokenExpiredException)
            {
                return true;
            }

            return false;
        }
        public string ExtractToken(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new ArgumentException("Authorization header cannot be null or empty.");
            }

            // Check if the header starts with "Bearer "
            if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // Return the token part of the header
                return authorizationHeader.Substring(7).Trim();
            }

            // If not a Bearer token, return the original header or handle as needed
            return authorizationHeader;
        }
        public IList<Claim> GetClaims(string token)
        {
            token = ExtractToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Validate token format and expiration
                if (string.IsNullOrWhiteSpace(token) || !tokenHandler.CanReadToken(token))
                {
                    throw new ArgumentException("Invalid token format");
                }

                // Optionally, validate token using the secret key
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // Set to true and provide Issuer if needed
                    ValidateAudience = false, // Set to true and provide Audience if needed
                    ValidateLifetime = true, // Ensures the token is not expired
                    ValidateIssuerSigningKey = true, // Ensures the token is signed correctly
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kL#Ia1FpmyJPo!'0;l%{TPJzJ@e5-,5_Tyb4nF"))
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract claims from the validated token
                var jwtToken = validatedToken as JwtSecurityToken;
                return jwtToken?.Claims.ToList() ?? new List<Claim>();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Token reading failed", ex);
            }
        }
    }
}
