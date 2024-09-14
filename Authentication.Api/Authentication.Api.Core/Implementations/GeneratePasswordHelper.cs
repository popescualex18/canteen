using AuthenticationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Implementations
{
    public class GeneratePasswordHelper : IGeneratePasswordHelper
    {
        public string GetSalt()
        {
            var bytes = new byte[5];
            using var keyGenerator = RandomNumberGenerator.Create();
            keyGenerator.GetBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public string GetHash(string text)
        {
            using var sha1 = SHA1.Create();
            var hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
