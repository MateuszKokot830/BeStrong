using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private static readonly Dictionary<ErrorType, int> _statusCodes = new()
        {
            [ErrorType.Validation]   = StatusCodes.Status400BadRequest,
            [ErrorType.Unauthorized] = StatusCodes.Status401Unauthorized,
            [ErrorType.NotFound]     = StatusCodes.Status404NotFound,
            [ErrorType.Conflict]     = StatusCodes.Status409Conflict,
            [ErrorType.Failure]      = StatusCodes.Status422UnprocessableEntity,
            [ErrorType.Unexpected]   = StatusCodes.Status500InternalServerError,
        };

        protected IActionResult Problem(List<Error> errors)
        {
            var firstError = errors[0];
            var statusCode = _statusCodes.GetValueOrDefault(firstError.Type, StatusCodes.Status500InternalServerError);

            if (firstError.Type == ErrorType.Validation)
            {
                var validationErrors = errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

                return ValidationProblem(new ValidationProblemDetails(validationErrors)
                {
                    Status = statusCode
                });
            }

            return Problem(statusCode: statusCode, title: firstError.Code, detail: firstError.Description);
        }
    }
}
