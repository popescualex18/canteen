using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCore.Interfaces
{
    public interface ITokenHelper
    {
        public string GenerateToken(string userId, string userEmail, string role, DateTime? expirationDate, string secret);
        public ClaimsPrincipal GetPrincipalFromToken(string token, string secret, bool isRefreshCall = false);
        public bool IsTokenExpired(string token, string secret);
        IList<Claim> GetClaims(string token);
    }
}
