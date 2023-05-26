using UrlShortener.Models.Dto;
using UrlShortener.Models.Enteties;

namespace UrlShortenerApi.Host.Repositories.Interfaces
{
    public interface IUrlRepository
    {
        Task AddUrl(Url url);
        Task<Url> CreateUrl(Url url);
        Task DeleteUrl(Url url);
        Task<IEnumerable<Url>> GetAllAsync();
        Task<string> GetLongUrlByShortAsync(string shortCode);
        Task<Url> GetUrlAsync(Url request);
        Task<Url> GetUrlByIdAsync(Guid id);
        Task<Url> GetUrlByOriginalStringAsync(string originalUrl);
    }
}