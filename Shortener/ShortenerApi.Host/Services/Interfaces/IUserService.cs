using UrlShortener.Models.Dto;
using UrlShortener.Models.Enums;
using UrlShortener.Models.Request;
using UrlShortener.Models.Response;

namespace UrlShortenerApi.Host.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(AuthenticateRequest request);
        Task<UserDto> GetUserById(Guid id);
        Task<Role> GetRoleAsync(Guid id);
    }
}