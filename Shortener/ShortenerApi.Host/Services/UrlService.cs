using AutoMapper;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;
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
        private readonly IConfiguration _configuration;
        private readonly IUrlRepository _urlRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UrlService(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<BaseDataService<ApplicationDbContext>> logger, IUrlRepository urlRepository, IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
            : base(dbContextWrapper, logger)
        {
            _userRepository = userRepository;
            _urlRepository = urlRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task AddUrlAsync(UrlDto url)
        {
            if (! await CheckWhetherUserCanEditInfo(url.CreatedById))
            {
                throw new BusinessException("The user cannot add url", 403);
            }

            var urlEntity = await ExecuteSafeAsync(() => _urlRepository.GetUrlByOriginalStringAsync(url.OriginalUrl));
            if (urlEntity is not null)
            {
                throw new BusinessException("This url is already exist", 400);
            }

            if (url.ShortenedUrl.IsNullOrEmpty())
            {
                url.ShortenedUrl = EncryptLongUrlToShortCode(url.OriginalUrl);
            }

            await ExecuteSafeAsync(() => _urlRepository.AddUrl(_mapper.Map<Url>(url)));
        }

        public async Task DeleteUrlAsync(UrlDto request)
        {
            if (!await CheckWhetherUserCanEditInfo(request.CreatedById))
            {
                throw new BusinessException("The user cannot delete url", 403);
            }

            var url = await ExecuteSafeAsync(() => _urlRepository.GetUrlAsync(_mapper.Map<Url>(request)));
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
            Url url = new Url()
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = EncryptLongUrlToShortCode(request.OriginalUrl),
                CreatedById = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            Url createdUrl = await ExecuteSafeAsync(() => _urlRepository.CreateUrl(url));
            UrlDto urlDto = _mapper.Map<UrlDto>(createdUrl);
            return urlDto;
        }

        private string EncryptLongUrlToShortCode(string longUrl)
        {
            if (!Uri.IsWellFormedUriString(longUrl, UriKind.Absolute))
            {
                throw new BusinessException("Invalid URL.", 400);
            }

            // Generate a short URL code. Here, we're using a timestamp for simplicity
            // Convert the current timestamp into a base64 string
            string shortCode = Convert.ToBase64String(BitConverter.GetBytes(DateTime.Now.Ticks)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return shortCode;
        }

        public async Task<UrlDto> GetUrl(UrlDto request)
        {
            var url = await ExecuteSafeAsync(() => _urlRepository.GetUrlAsync(_mapper.Map<Url>(request)));
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

        private async Task<bool> CheckWhetherUserCanEditInfo(Guid userId)
        {
            return (await _userRepository.GetUserByIdAsync(userId)).UserRole == UrlShortener.Models.Enums.Role.Admin;
        }
    }
}
