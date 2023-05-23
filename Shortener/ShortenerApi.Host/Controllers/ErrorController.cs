using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerApi.Host.Controllers
{
    [Route(ComponentDefaults.DefaultRoute)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("notfound")]
        public ActionResult Error404()
        {
            return NotFound(new ProblemDetails
            {
                Title = "Resource not found",
                Detail = "The requested resource could not be found"
            });
        }

        [HttpGet("servererror")]
        public ActionResult Error500()
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Server error",
                Detail = "An unexpected error occurred on the server"
            });
        }
    }
}
