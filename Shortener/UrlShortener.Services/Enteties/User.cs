using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Enums;

namespace UrlShortener.Models.Enteties
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password must be between 1 and 255 characters.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public Role UserRole { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public User()
        {
        }

        public User(string username, string password, Role role, DateTime createdAt)
        {
            UserName = username;
            UserRole = role;
            CreatedAt = createdAt;
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            this.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public User(Guid id, string username, string password, Role role, DateTime createdAt)
        {
            Id = id;
            UserName = username;
            UserRole = role;
            CreatedAt = createdAt;
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            this.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
