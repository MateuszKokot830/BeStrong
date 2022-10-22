using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IAppUserService _appUserService;
        private readonly ITokenService _tokenService;
        public UsersController(IAppUserService appUserService, ITokenService tokenService)
        {
            _appUserService = appUserService;
            _tokenService = tokenService;
        }

        [SwaggerOperation(Summary = "Retrieves all users")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        {
            var users = await _appUserService.GetAllUsers();
            return users.ToList();
        }

        [SwaggerOperation(Summary = "Retrieves a specific user by unique id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserDto>> GetUser(int id)
        {
            return await _appUserService.GetUserById(id);
        }

        [SwaggerOperation(Summary = "Creates a user with given username and password")]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _appUserService.GetUserByUsername(registerDto.Username);
            if (user != null) return BadRequest("Username is taken");

            user = await _appUserService.AddUser(registerDto);

            return new UserDto{Username = user.Username, Token = _tokenService.CreateToken(user)};
        }

        [SwaggerOperation(Summary = "Login a user with given username and password")]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _appUserService.GetUserByUsername(loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            var isPasswordCorrect = _appUserService.IsPasswordCorrect(user, loginDto);
            return isPasswordCorrect ? new UserDto{Username = user.Username, Token = _tokenService.CreateToken(user)} 
                : Unauthorized("Invalid password");
        }
    }
}