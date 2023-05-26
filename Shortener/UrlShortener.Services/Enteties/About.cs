using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Enteties
{
    public class About
    {
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
    }
}
