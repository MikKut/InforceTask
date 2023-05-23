using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Enteties;

namespace UrlShortener.Models.Request
{
    public class GetItemGyIdRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
