using IdentityServer.Models;
using System.Numerics;

namespace IdentityServer.Services
{
    public class RoleCheckService
    {
        public string GetRoleName(BigInteger roleIdentifier)
        {
            if (roleIdentifier == Role.Admin)
            {
                return "Admin";
            }

            if (roleIdentifier == Role.User)
            {
                return "User";
            }

            return "Unathorized";
        }
    }
    internal class Role
    {
        public static BigInteger Unathorized { get => BigInteger.Zero; }
        public static BigInteger User { get => BigInteger.Parse("8942850368293359232293816813965187505045032701936306256035712393274178904"); }
        public static BigInteger Admin { get => BigInteger.Parse("9347912478349681347910273409812364981237"); }
    }
}
