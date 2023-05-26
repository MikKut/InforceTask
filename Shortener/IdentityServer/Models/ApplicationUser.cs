using Microsoft.AspNetCore.Identity;
using System.Numerics;

namespace IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public BigInteger RoleIdentifier { get; }
    }
}
