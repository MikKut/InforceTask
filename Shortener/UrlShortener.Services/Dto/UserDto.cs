using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Enums;

namespace UrlShortener.Models.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }

        public Guid Id { get; set;}

        public Role Role { get; set; }
    }
}
