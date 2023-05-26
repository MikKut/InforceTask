using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Enteties;
using UrlShortener.Models.Request;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;

namespace UrlShortenerApi.Host.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.SingleAsync(u => u.Id == id);
            return user is null ? throw new BusinessException("There is no such user", 404) : user;
        }

        public async Task<User> GetByUserCredentialsAsync(string userName, string password)
        {
            var users = _context.Users.Where(u => u.UserName == userName);

            if (users is null)
            {
                throw new BusinessException("There is no user with such name", 401);
            }

            foreach (var user in users)
            {
               if (user is not null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
               {
                   return user;
               }
            }

            throw new BusinessException("Wrong user credentials", 401); ; // User not found or password doesn't match
        }
    }
}
