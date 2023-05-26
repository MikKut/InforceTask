using Infrastructure.Exceptions;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Net;
using UrlShortener.Models.Request;
using UrlShortener.Models.Response;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class AboutController : ControllerBase
    {
        private readonly ILogger<AboutController> _logger;
        private readonly IAboutService _aboutService;
        private readonly IUserService _userService;

        public AboutController(IAboutService aboutService, IUserService userService, ILogger<AboutController> logger)
        {
            _logger = logger;
            _aboutService = aboutService;
            _userService=userService;
        }

        [HttpGet("GetAbout")]
        [ProducesResponseType(typeof(AboutResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAboutInfo()
        {
            try
            {
                string aboutInfo = await _aboutService.GetAboutInfoAwait();
                _logger.LogInformation($"Returned about info from {nameof(GetAboutInfo)} method of {nameof(AboutController)} controller");
                return Ok(
                    new AboutResponse() 
                    { 
                        Content = aboutInfo,
                    });
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(GetAboutInfo)} method of {nameof(AboutController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(GetAboutInfo)} method of {nameof(AboutController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("UpdateAbout")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAboutInfo(UpdateAboutRequest request)
        {
            try
            {
                bool userCanEddit = await _aboutService.CheckWhetherUserCanEdditAboutInfoAwait(request.UserId);
                string aboutInfo = await _aboutService.GetAboutInfoAwait();
                if (!userCanEddit)
                {
                    throw new BusinessException("User cannot edit the update view", 403);
                }

                bool isDoneSuccessfuly = await _aboutService.UpdateAboutInfoAwait(request.NewInfo);
                if (!isDoneSuccessfuly)
                {
                    throw new Exception("Something went wrong: you cannot update about info.");
                }

                _logger.LogInformation($"{nameof(UpdateAboutInfo)} method of {nameof(AboutController)} controller is done successfuly");
                return Ok(isDoneSuccessfuly);
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(UpdateAboutInfo)} method of {nameof(AboutController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(UpdateAboutInfo)} method of {nameof(AboutController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetUserAbilityToEdit/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserAbilityToEdit(string id)
        {
            try
            {
                Guid userId = new Guid(id);
                bool userCanEddit = await _aboutService.CheckWhetherUserCanEdditAboutInfoAwait(userId);
                return Ok(userCanEddit);

            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(GetUserAbilityToEdit)} method of {nameof(AboutController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(GetUserAbilityToEdit)} method of {nameof(AboutController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
