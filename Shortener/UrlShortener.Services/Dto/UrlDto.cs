using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Enteties;

namespace UrlShortener.Models.Dto
{
    public class UrlDto
    {
        public string OriginalUrl { get; set; } = null!;

        public string? ShortenedUrl { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
