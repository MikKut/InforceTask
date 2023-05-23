using UrlShortener.Models.Dto;
using UrlShortener.Models.Request;

namespace UrlShortenerApi.Host.Services.Interfaces
{
    public interface IUrlService
    {
        Task AddUrlAsync(UrlDto url);
        Task<UrlDto> CreateShortUrlAsync(UrlCreateRequest request);
        Task DeleteUrlAsync(UrlDto request);
        Task<IEnumerable<UrlDto>> GetAllAsync();
        Task<string> GetLongUrlByShortAsync(string shortCode);
        Task<UrlDto> GetUrl(UrlDto request);
        Task<UrlDto?> GetUrlByIdOrNullAsync(Guid id);
    }
}