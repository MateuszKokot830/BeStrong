using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Commands.Register;
using Application.Queries.Login;

namespace WebAPI.Controllers
{
    [Route("auth")]
    public class AuthenticationController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        public AuthenticationController(IMediator mediator, ITokenService tokenService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
        }

        [SwaggerOperation(Summary = "Creates a user with given username and password")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequestDto userRegisterRequestDto)
        {
            ErrorOr<UserAuthResponseDto> authResult = await _mediator.Send(new RegisterCommand() {
                userRegisterRequestDto = userRegisterRequestDto});

            return authResult.Match(
                authResult => Ok(authResult),
                errors => Problem(errors));
        }


        [SwaggerOperation(Summary = "Login a user with given username and password")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto userLoginRequestDto)
        {
            ErrorOr<UserAuthResponseDto> authResult = await _mediator.Send(new LoginQuery() {
                userLoginRequestDto = userLoginRequestDto});

            return authResult.Match(
                authResult => Ok(authResult),
                errors => Problem(errors));
        }
    }
}