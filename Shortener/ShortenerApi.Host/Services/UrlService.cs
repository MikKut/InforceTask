using AutoMapper;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Enteties;
using UrlShortener.Models.Request;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.Host.Services
{
    public class UrlService : BaseDataService<ApplicationDbContext>, IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IMapper _mapper;

        public UrlService(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<BaseDataService<ApplicationDbContext>> logger, IUrlRepository urlRepository, IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _urlRepository = urlRepository;
            _mapper = mapper;
        }

        public async Task AddUrlAsync(UrlDto url)
        {
            if (!CheckWhetherUserCanEditInfo(url.CreatedBy))
            {
                throw new BusinessException("The user cannot delete url", 403);
            }

            var urlEntity = await ExecuteSafeAsync(() => _urlRepository.GetUrlByOriginalStringAsync(url.OriginalUrl));
            if (urlEntity is not null)
            {
                throw new BusinessException("This url is already exist", 400);
            }

            await ExecuteSafeAsync(() => _urlRepository.AddUrl(_mapper.Map<Url>(url)));
        }

        public async Task DeleteUrlAsync(UrlDto request)
        {
            if (!CheckWhetherUserCanEditInfo(request.CreatedBy))
            {
                throw new BusinessException("The user cannot delete url", 403);
            }

            var url = await ExecuteSafeAsync(() => _urlRepository.GetUrlAsync(request));
            if (url == null)
            {
                throw new BusinessException("URL not found.", 400);
            }

            await _urlRepository.DeleteUrl(url);
        }

        public async Task<IEnumerable<UrlDto>> GetAllAsync()
        {
            IEnumerable<Url> allUrls = await ExecuteSafeAsync(() => _urlRepository.GetAllAsync());
            if (allUrls is null)
            {
                return new List<UrlDto>();
            }

            return _mapper.Map<IEnumerable<UrlDto>>(allUrls);
        }

        public async Task<UrlDto> CreateShortUrlAsync(UrlCreateRequest request)
        {
            if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
            {
                throw new BusinessException("Invalid URL.", 400);
            }

            // Generate a short URL code. Here, we're using a timestamp for simplicity
            // Convert the current timestamp into a base64 string
            string shortCode = Convert.ToBase64String(BitConverter.GetBytes(DateTime.Now.Ticks)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            Url url = new Url()
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
                CreatedById = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            Url createdUrl = await ExecuteSafeAsync(() => _urlRepository.CreateUrl(url));
            UrlDto urlDto = _mapper.Map<UrlDto>(createdUrl);
            return urlDto;
        }

        public async Task<UrlDto> GetUrl(UrlDto request)
        {
            var url = await ExecuteSafeAsync(() => _urlRepository.GetUrlAsync(request));
            if (url == null)
            {
                throw new BusinessException("URL not found.");
            }

            return _mapper.Map<UrlDto>(url);
        }

        public async Task<string> GetLongUrlByShortAsync(string shortCode)
        {

            string url = await ExecuteSafeAsync(() => _urlRepository.GetLongUrlByShortAsync(shortCode));
            if (string.IsNullOrEmpty(url))
            {
                throw new BusinessException("URL not found.", 400);
            }

            return url;
        }

        public async Task<UrlDto?> GetUrlByIdOrNullAsync(Guid id)
        {
            Url url = await ExecuteSafeAsync(() => _urlRepository.GetUrlByIdAsync(id));
            if (url is null)
            {
                return null;
            }

            return _mapper.Map<UrlDto>(url);
        }

        private bool CheckWhetherUserCanEditInfo(UserDto user)
        {
            return user.Role == UrlShortener.Models.Enums.Role.Admin;
        }
    }
}
