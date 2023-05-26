using System;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using UrlShortener.Models.Enteties;
using UrlShortener.Models.Enums;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Services;
using Xunit;

namespace UrlShortenerApi.UnitTests.Services
{
    public class AboutServiceTests
    {
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAboutRepository> _aboutRepositoryMock;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly AboutService _aboutService;

        public AboutServiceTests()
        {
            _loggerMock = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);
            _userRepositoryMock = new Mock<IUserRepository>();
            _aboutRepositoryMock = new Mock<IAboutRepository>();

            _aboutService = new AboutService(
                _dbContextWrapper.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                _aboutRepositoryMock.Object);
        }

        [Fact]
        public async Task CheckWhetherUserCanEditAboutInfoAsync_ExistingUser_ReturnsTrueForAdmin()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var user = new User { Id = userId, UserRole = Role.Admin };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            bool result = await _aboutService.CheckWhetherUserCanEdditAboutInfoAwait(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckWhetherUserCanEditAboutInfoAsync_ExistingUser_ReturnsFalseForNonAdmin()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var user = new User { Id = userId, UserRole = Role.User };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            bool result = await _aboutService.CheckWhetherUserCanEdditAboutInfoAwait(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckWhetherUserCanEditAboutInfoAsync_NonExistingUser_ThrowsBusinessException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act and Assert
            await Assert.ThrowsAsync<BusinessException>(() => _aboutService.CheckWhetherUserCanEdditAboutInfoAwait(userId));
        }

        [Fact]
        public async Task GetAboutInfoAsync_AboutInfoExists_ReturnsAboutInfo()
        {
            // Arrange
            string aboutInfo = "About Info";
            _aboutRepositoryMock.Setup(repo => repo.GetAboutInfoAsync()).ReturnsAsync(aboutInfo);

            // Act
            string result = await _aboutService.GetAboutInfoAwait();

            // Assert
            Assert.Equal(aboutInfo, result);
        }

        [Fact]
        public async Task GetAboutInfoAsync_AboutInfoDoesNotExist_ThrowsBusinessException()
        {
            // Arrange
            _aboutRepositoryMock.Setup(repo => repo.GetAboutInfoAsync()).ReturnsAsync((string)null);

            // Act and Assert
            await Assert.ThrowsAsync<BusinessException>(() => _aboutService.GetAboutInfoAwait());
        }

        [Fact]
        public async Task UpdateAboutInfoAsync_UpdateSuccessful_ReturnsTrue()
        {
            // Arrange
            string newInfo = "New About Info";
            _aboutRepositoryMock.Setup(repo => repo.UpdateAboutInfoAsync(newInfo)).ReturnsAsync(true);

            // Act
            bool result = await _aboutService.UpdateAboutInfoAwait(newInfo);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAboutInfoAsync_UpdateFailed_ReturnsFalse()
        {
            // Arrange
            string newInfo = "New About Info";
            _aboutRepositoryMock.Setup(repo => repo.UpdateAboutInfoAsync(newInfo)).ReturnsAsync(false);

            // Act
            bool result = await _aboutService.UpdateAboutInfoAwait(newInfo);

            // Assert
            Assert.False(result);
        }
    }
}