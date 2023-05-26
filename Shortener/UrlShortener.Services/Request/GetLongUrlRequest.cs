using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Request
{
    public class GetLongUrlRequest
    {
        [Required(ErrorMessage = "Shortened URL is required.")]
        [StringLength(2000, ErrorMessage = "Shorten code must be between 1 and 2000 characters.")]
        public string ShortCode { get; set; }
    }
}
