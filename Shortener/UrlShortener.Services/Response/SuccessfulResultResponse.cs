using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Response
{
    public class SuccessfulResultResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
