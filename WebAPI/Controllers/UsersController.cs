using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Queries.Users;
using Application.Commands.Users;

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
        public async Task<ActionResult<IEnumerable<UserAggregateDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return users.ToList();
        }


        [SwaggerOperation(Summary = "Retrieves a specific user by unique id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAggregateDto>> GetUser(int id)
        {
            return await _mediator.Send(new GetUserByIdQuery() {Id = id});
        }

    }
}