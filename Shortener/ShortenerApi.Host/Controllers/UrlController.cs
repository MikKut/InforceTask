using Infrastructure;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Request;
using UrlShortener.Models.Response;
using UrlShortenerApi.Host.Services.Interfaces;

namespace UrlShortenerApi.Host.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly ILogger<UrlController> _logger;

        public UrlController(IUrlService urlService, ILogger<UrlController> logger)
        {
            _urlService = urlService;
            _logger=logger;
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var urls = await _urlService.GetAllAsync();
                return Ok(urls);
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(GetAll)} method of {nameof(UrlController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(GetAll)} method of {nameof(UrlController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("AddUrl")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddUrl(UrlDto request)
        {
            try
            {
                await _urlService.AddUrlAsync(request);
                return Ok();
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("ShortenUrl")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ShortenUrl(UrlCreateRequest request)
        {
            try
            {
                UrlDto newUrlDto = await _urlService.CreateShortUrlAsync(request);
                _logger.LogInformation($"Url shortening was done successfuly");
                return Ok(newUrlDto);
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("DeleteUrl")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUrl(UrlDto request)
        {
            try
            {
                await _urlService.DeleteUrlAsync(request);
                _logger.LogInformation($"Url deleting was done successfuly");
                return Ok();
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("GetLongUrlByShorten/{shortCode}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLongUrlByShorten(string shortCode)
        {
            try
            {
                string longUrl = await _urlService.GetLongUrlByShortAsync(shortCode);
                _logger.LogInformation($"Url deleting was done successfuly");
                return Ok(longUrl);
            }
            catch (BusinessException exception)
            {
                _logger.LogWarning($"Business exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return StatusCode(exception.StatusCode, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception in {nameof(ShortenUrl)} method of {nameof(UrlController)} controller: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
