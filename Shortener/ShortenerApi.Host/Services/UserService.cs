using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Enums;
using UrlShortener.Models.Request;
using UrlShortener.Models.Response;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.Host.Services
{
    public class UserService : BaseDataService<ApplicationDbContext>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<BaseDataService<ApplicationDbContext>> logger, IUserRepository userRepository)
            : base(dbContextWrapper, logger)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Authenticate(AuthenticateRequest model)
        {
            var user = await ExecuteSafeAsync(() => _userRepository.GetByUserCredentialsAsync(model.UserName, model.UserPassword));

            if (user == null)
            {
                throw new BusinessException("Invalid username or password");
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.UserRole
            };
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await ExecuteSafeAsync(() => _userRepository.GetUserByIdAsync(id));
            if (user == null)
            {
                throw new BusinessException("User not found");
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.UserRole
            };
        }

        public async Task<Role> GetRoleAsync(Guid id)
        {
            var user = await ExecuteSafeAsync(() => _userRepository.GetUserByIdAsync(id));
            if (user == null)
            {
                throw new BusinessException("User not found", 404);
            }

            return user.UserRole;
        }
    }

}
