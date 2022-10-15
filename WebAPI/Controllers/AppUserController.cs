using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [SwaggerOperation(Summary = "Retrieves all users")]
        [HttpGet]
        public IActionResult Get()
        {
            var users = _appUserService.GetAllUsers();
            return Ok(users);
        }

        [SwaggerOperation(Summary = "Retrieves a specific user by unique id")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _appUserService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}