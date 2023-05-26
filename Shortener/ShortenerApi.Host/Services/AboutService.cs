using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using UrlShortener.Models.Enteties;
using UrlShortener.Models.Enums;
using UrlShortener.Models.Request;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.Host.Services
{
    public class AboutService 
        : BaseDataService<ApplicationDbContext>, IAboutService
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDbContextWrapper<ApplicationDbContext> _dbContextWrapper;

        public AboutService(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<BaseDataService<ApplicationDbContext>> logger, IUserRepository userRepository, IAboutRepository aboutRepository) 
            : base(dbContextWrapper, logger)
        {
            _aboutRepository = aboutRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> CheckWhetherUserCanEdditAboutInfoAwait(Guid userId)
        {
            User result = await ExecuteSafeAsync(() => _userRepository.GetUserByIdAsync(userId));
            if (result is null)
            {
                throw new BusinessException("There is no such user", 404);   
            }

            if (result.UserRole != Role.Admin)
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetAboutInfoAwait()
        {
            string? result = await ExecuteSafeAsync(() => _aboutRepository.GetAboutInfoAsync());
            if (string.IsNullOrEmpty(result))
            {
                throw new BusinessException("There is no about info", 404);
            }

            return result!;
        }

        public async Task<bool> UpdateAboutInfoAwait(string newInfo)
        {
            var result = await ExecuteSafeAsync(() => _aboutRepository.UpdateAboutInfoAsync(newInfo));
            return result;
        }
    }

}
