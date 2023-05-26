using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Response
{
    public class AboutResponse
    {
        [Required]
        public string Content { get; set; }
        public bool IsAllowedToEdit { get; set; }
    }
}
