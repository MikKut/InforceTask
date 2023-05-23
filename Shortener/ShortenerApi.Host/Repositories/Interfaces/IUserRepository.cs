using UrlShortener.Models.Enteties;
using UrlShortener.Models.Enums;
using UrlShortener.Models.Request;

namespace UrlShortenerApi.Host.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUserCredentialsAsync(string userName, string password);
        Task<User> GetUserByIdAsync(Guid id);
    }
}