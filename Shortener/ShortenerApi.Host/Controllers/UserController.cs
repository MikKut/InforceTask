using Infrastructure.Exceptions;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Request;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger=logger;
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            try
            {
                UserDto response = await _userService.Authenticate(model); ;
                return Ok(response);

            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(Authenticate)} method of {nameof(UserController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(Authenticate)} method of {nameof(UserController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var response = await _userService.GetUserById(id);
                return Ok(response);
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(GetUserById)} method of {nameof(UserController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(GetUserById)} method of {nameof(UserController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
