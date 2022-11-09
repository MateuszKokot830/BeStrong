using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Queries.Users;
using Application.Commands.Users;
using Application.Commands.Authentication;

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
        public async Task<ActionResult<UserAuthResponseDto>> Register(UserRegisterRequestDto userRegisterRequestDto)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery() {Username = userRegisterRequestDto.Username});
            if (user != null) return BadRequest("Username is taken");

            user = await _mediator.Send(new CreateUserCommand(){UserRegisterRequestDto = userRegisterRequestDto});

            return new UserAuthResponseDto 
                {
                    Username = user.Username, 
                    Token = _tokenService.CreateToken(user)
                };
        }


        [SwaggerOperation(Summary = "Login a user with given username and password")]
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthResponseDto>> Login(UserLoginRequestDto userLoginRequestDto)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery() {Username = userLoginRequestDto.Username});
            if (user == null) return Unauthorized("Invalid username");

            var isPasswordCorrect = await _mediator.Send(new AuthenticateUserCommand() {
                UserAggregateDto = user, UserLoginRequestDto = userLoginRequestDto});

            return isPasswordCorrect ? new UserAuthResponseDto
                {   
                    Username = user.Username, 
                    Token = _tokenService.CreateToken(user)
                } 
                : Unauthorized("Invalid password");
        }
    }
}