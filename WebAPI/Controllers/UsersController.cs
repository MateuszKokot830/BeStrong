using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        public UsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [SwaggerOperation(Summary = "Retrieves all users")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _appUserService.GetAllUsers();
            return Ok(users);
        }

        [SwaggerOperation(Summary = "Retrieves a specific user by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _appUserService.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
}