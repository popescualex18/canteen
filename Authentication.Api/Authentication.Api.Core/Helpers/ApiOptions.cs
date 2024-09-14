using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Helpers
{
    public class ApiOptions
    {
        public TokenOptions TokenOptions { get; set; }
        public TokenOptions RefreshTokenOptions { get; set; }
    }
}
