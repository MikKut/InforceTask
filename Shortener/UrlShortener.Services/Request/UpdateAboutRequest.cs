using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Request
{
    public class UpdateAboutRequest
    {
        public Guid UserId { get; set; }
        public string NewInfo { get; set; }
    }
}
