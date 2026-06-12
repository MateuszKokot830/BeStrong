using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            return User.GetUserId();
        }

        protected string GetCurrentUsername()
        {
            return User.GetUsername();
        }

        protected IActionResult Problem(List<Error> errors)
        {
            var error = errors[0];

            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status422UnprocessableEntity,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };

            return ValidationProblem(new ValidationProblemDetails(
                errors.GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray())
));
        }
    }
}