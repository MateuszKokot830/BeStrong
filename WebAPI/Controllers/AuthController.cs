using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Commands.Register;
using Application.Queries.Login;
using Application.Dto.Auth;

namespace WebAPI.Controllers
{
    public class AuthController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a user with given username and password")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequestDto userRegisterRequestDto)
        {
            ErrorOr<UserAuthResponseDto> authResult = await _mediator.Send(new RegisterCommand(userRegisterRequestDto));

            return authResult.Match(
                authResult => Ok(authResult),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Login a user with given username and password")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto userLoginRequestDto)
        {
            ErrorOr<UserAuthResponseDto> authResult = await _mediator.Send(new LoginQuery(userLoginRequestDto));

            return authResult.Match(
                authResult => Ok(authResult),
                errors => Problem(errors));
        }
    }
}