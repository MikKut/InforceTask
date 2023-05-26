using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Request
{
    public class AuthenticateRequest
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
