using System;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using UrlShortener.Models.Enteties;
using UrlShortener.Models.Enums;
using UrlShortener.Models.Request;
using UrlShortener.Models.Response;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Services;
using Xunit;
using Microsoft.EntityFrameworkCore.Storage;

namespace UrlShortenerApi.UnitTests.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapperMock;
        private Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerMock;

        private UserService _userService;

        public UserServiceTests()
        {
            _dbContextWrapperMock = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapperMock.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();

            _userService = new UserService(
                _dbContextWrapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsUserDto()
        {
            // Arrange
            var model = new AuthenticateRequest
            {
                UserName = "testuser",
                UserPassword = "password"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                UserRole = Role.User
            };

            _userRepositoryMock.Setup(repo => repo.GetByUserCredentialsAsync(model.UserName, model.UserPassword))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.Authenticate(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.UserRole, result.Role);
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_ThrowsBusinessException()
        {
            // Arrange
            var model = new AuthenticateRequest
            {
                UserName = "testuser",
                UserPassword = "password"
            };

            _userRepositoryMock.Setup(repo => repo.GetByUserCredentialsAsync(model.UserName, model.UserPassword))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() => _userService.Authenticate(model));
        }

        [Fact]
        public async Task GetUserById_ExistingUser_ReturnsUserDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                UserName = "testuser",
                UserRole = Role.User
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.UserRole, result.Role);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ThrowsBusinessException()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() => _userService.GetUserById(id));
        }

        [Fact]
        public async Task GetRoleAsync_ExistingUser_ReturnsUserRole()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                UserName = "testuser",
                UserRole = Role.User
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetRoleAsync(id);

            // Assert
            Assert.Equal(user.UserRole, result);
        }

        [Fact]
        public async Task GetRoleAsync_NonExistingUser_ThrowsBusinessException()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() => _userService.GetRoleAsync(id));
        }
    }
}
