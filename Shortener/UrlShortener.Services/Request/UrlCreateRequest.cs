using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Request
{
    public class UrlCreateRequest
    {
        [Required(ErrorMessage = "Original URL is required.")]
        [StringLength(2000, ErrorMessage = "Original URL must be between 1 and 2000 characters.")]
        public string OriginalUrl { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
