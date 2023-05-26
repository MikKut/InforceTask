using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Request
{
    public class UrlRequest
    {
        public string UserId { get; set; }
        public string UrlId { get; set; }
    }
}
