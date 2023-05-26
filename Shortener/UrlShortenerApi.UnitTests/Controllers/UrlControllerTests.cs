using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Request;
using UrlShortenerApi.Host.Controllers;
using UrlShortenerApi.Host.Services.Interfaces;
using Xunit.Sdk;

namespace UrlShortenerApi.UnitTests.Controllers
{
    public class UrlControllerTests
    {
        private readonly Mock<IUrlService> _urlService;
        private readonly Mock<ILogger<UrlController>> _logger;
        private readonly UrlController _controller;

        public UrlControllerTests()
        {
            _urlService = new Mock<IUrlService>();
            _logger = new Mock<ILogger<UrlController>>();
            _controller = new UrlController(_urlService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenCalled()
        {
            // Arrange
            _urlService.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<UrlDto>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUrl_ReturnsOk_WhenCalledWithValidUrlDto()
        {
            // Arrange
            var urlDto = new UrlDto
            {
                OriginalUrl = "http://example.com",
                CreatedById = It.IsAny<Guid>()
            };
            _urlService.Setup(x => x.DeleteUrlAsync(It.IsAny<UrlDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUrl(urlDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _urlService.Verify(x => x.DeleteUrlAsync(It.IsAny<UrlDto>()), Times.Once);
        }

        [Fact]
        public async Task GetLongUrlByShorten_ReturnsOk_WhenCalledWithValidGetLongUrlRequest()
        {
            // Arrange
            string shortCode = "abcd";
            _urlService.Setup(x => x.GetLongUrlByShortAsync(It.IsAny<string>())).ReturnsAsync("http://example.com");

            // Act
            var result = await _controller.GetLongUrlByShorten(shortCode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("http://example.com", okResult.Value);
            _urlService.Verify(x => x.GetLongUrlByShortAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddUrl_ReturnsOk_WhenCalledWithValidUrlDto()
        {
            // Arrange
            var urlDto = new UrlDto
            {
                OriginalUrl = "http://example.com",
                CreatedById = It.IsAny<Guid>()
            };
            _urlService.Setup(x => x.AddUrlAsync(It.IsAny<UrlDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddUrl(urlDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _urlService.Verify(x => x.AddUrlAsync(It.IsAny<UrlDto>()), Times.Once);
        }

        [Fact]
        public async Task ShortenUrl_ReturnsOk_WhenCalledWithValidUrlCreateRequest()
        {
            // Arrange
            var urlCreateRequest = new UrlCreateRequest
            {
                OriginalUrl = "http://example.com",
                UserId = Guid.NewGuid()
            };
            var expectedUrlDto = new UrlDto
            {
                OriginalUrl = "http://example.com",
                CreatedById = new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF")
            };
            _urlService.Setup(x => x.CreateShortUrlAsync(It.IsAny<UrlCreateRequest>())).ReturnsAsync(expectedUrlDto);

            // Act
            var result = await _controller.ShortenUrl(urlCreateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUrlDto = Assert.IsType<UrlDto>(okResult.Value);
            Assert.Equal(expectedUrlDto.OriginalUrl, returnUrlDto.OriginalUrl);
            Assert.Equal(expectedUrlDto.CreatedById, returnUrlDto.CreatedById);
            _urlService.Verify(x => x.CreateShortUrlAsync(It.IsAny<UrlCreateRequest>()), Times.Once);
        }

        [Fact]
        public async Task AddUrl_ReturnsBadRequest_WhenCalledWithInvalidUrlDto()
        {
            // Arrange
            var urlDto = new UrlDto();
            _urlService.Setup(x => x.AddUrlAsync(urlDto)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.AddUrl(urlDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _urlService.Verify(x => x.AddUrlAsync(urlDto), Times.Once);
        }

        [Fact]
        public async Task AddUrl_ReturnsBadRequest_WhenCalledWithWrongRoleInUrlDto()
        {
            // Arrange
            var urlDto = new UrlDto();
            _urlService.Setup(x => x.AddUrlAsync(urlDto)).ThrowsAsync(new BusinessException("Test exception", 403));

            // Act
            var result = await _controller.AddUrl(urlDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, objectResult.StatusCode);
            Assert.Equal("Test exception", objectResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _urlService.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsStatusCode_WhenBusinessExceptionIsThrown()
        {
            // Arrange
            _urlService.Setup(x => x.GetAllAsync()).ThrowsAsync(new BusinessException("Test exception", 403));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, objectResult.StatusCode);
            Assert.Equal("Test exception", objectResult.Value);
        }
    }
}
