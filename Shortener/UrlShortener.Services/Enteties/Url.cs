using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Models.Enteties
{
    public class Url
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Original URL is required.")]
        [StringLength(2000, ErrorMessage = "Original URL must be between 1 and 2000 characters.")]
        public string OriginalUrl { get; set; }

        [Required(ErrorMessage = "Shortened URL is required.")]
        [StringLength(2000, ErrorMessage = "Shorten code must be between 1 and 2000 characters.")]
        public string ShortCode { get; set; }

        [Required(ErrorMessage = "CreatedBy is required.")]
        public Guid CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }

        [Required(ErrorMessage = "CreatedAt is required.")]
        public DateTime CreatedAt { get; set; }
    }
}
