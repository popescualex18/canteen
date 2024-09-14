using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Helpers
{
    public class TokenOptions
    {
        public string Secret { get; set; }
        public int HoursUntilExpiration { get; set; }
    }
}
