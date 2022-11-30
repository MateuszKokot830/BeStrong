using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Queries.Users.GetUser;
using Application.Queries.Users.GetUsers;

namespace WebAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        public UsersController(IMediator mediator, ITokenService tokenService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
        }


        [SwaggerOperation(Summary = "Retrieves all users")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return users.ToList();
        }


        [SwaggerOperation(Summary = "Retrieves a specific user by unique id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            return await _mediator.Send(new GetUserQuery() {Id = id});
        }

    }
}