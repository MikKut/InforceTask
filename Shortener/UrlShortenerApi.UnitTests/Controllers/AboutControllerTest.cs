using Castle.Components.DictionaryAdapter.Xml;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Models.Request;
using UrlShortener.Models.Response;
using UrlShortenerApi.Host.Controllers;
using UrlShortenerApi.Host.Services;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.UnitTests.Controllers
{
    public class AboutControllerTests
    {
        private readonly AboutController _controller;
        private readonly Mock<IAboutService> _mockAboutService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<AboutController>> _mockLogger;

        public AboutControllerTests()
        {
            _mockAboutService = new Mock<IAboutService>();
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<AboutController>>();
            _controller = new AboutController(_mockAboutService.Object, _mockUserService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAboutInfo_ReturnsAboutResponse_WhenRequestIsValid()
        {
            // Arrange
            _mockAboutService.Setup(service => service.CheckWhetherUserCanEdditAboutInfoAwait(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockAboutService.Setup(service => service.GetAboutInfoAwait()).ReturnsAsync("Test info");

            // Act
            var result = await _controller.GetAboutInfo();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var aboutResponse = Assert.IsType<AboutResponse>(okResult.Value);
            Assert.Equal("Test info", aboutResponse.Content);
        }

        [Fact]
        public async Task UpdateAboutInfo_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateAboutRequest = new UpdateAboutRequest { UserId = Guid.NewGuid(), NewInfo = "Updated Info" };
            _mockAboutService.Setup(service => service.CheckWhetherUserCanEdditAboutInfoAwait(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockAboutService.Setup(service => service.UpdateAboutInfoAwait(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateAboutInfo(updateAboutRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdateAboutInfo_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var updateAboutRequest = new UpdateAboutRequest { UserId = Guid.NewGuid(), NewInfo = "Updated Info" };
            _mockAboutService.Setup(service => service.CheckWhetherUserCanEdditAboutInfoAwait(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockAboutService.Setup(service => service.UpdateAboutInfoAwait(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateAboutInfo(updateAboutRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Something went wrong: you cannot update about info.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetUserAbilityToEdit_ReturnsTrue_WhenUserCanEdit()
        {
            // Arrange
            var getItemGyIdRequest = new Guid().ToString();
            _mockAboutService.Setup(service => service.CheckWhetherUserCanEdditAboutInfoAwait(It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            var result = await _controller.GetUserAbilityToEdit(getItemGyIdRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task GetUserAbilityToEdit_ReturnsFalse_WhenUserCannotEdit()
        {
            // Arrange
            var getItemGyIdRequest = new Guid().ToString();
            _mockAboutService.Setup(service => service.CheckWhetherUserCanEdditAboutInfoAwait(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            var result = await _controller.GetUserAbilityToEdit(getItemGyIdRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.False((bool)okResult.Value);
        }

        [Fact]
        public async Task GetAboutInfo_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockAboutService.Setup(x => x.GetAboutInfoAwait()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAboutInfo();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAboutInfo_ReturnsStatusCode_WhenBusinessExceptionIsThrown()
        {
            // Arrange
            _mockAboutService.Setup(x => x.GetAboutInfoAwait()).ThrowsAsync(new BusinessException("Test exception", 404));

            // Act
            IActionResult result = await _controller.GetAboutInfo();
            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.True((result as StatusCodeResult)?.StatusCode == 404);

        }
    }

}
