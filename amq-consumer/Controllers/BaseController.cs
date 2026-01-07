using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Success(object data)
        {
            return Ok(new
            {
                success = true,
                data
            });
        }

        protected IActionResult CreatedResponse(object data)
        {
            return Created(string.Empty, new
            {
                success = true,
                data
            });
        }

        protected IActionResult Error(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new
            {
                success = false,
                error = message
            });
        }
    }
}
