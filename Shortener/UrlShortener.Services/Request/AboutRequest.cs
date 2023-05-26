using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Dto;

namespace UrlShortener.Models.Request
{
    public class AboutRequest
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
