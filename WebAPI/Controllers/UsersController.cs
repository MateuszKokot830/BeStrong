using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Queries.Users.GetUser;
using Application.Queries.Users.GetUsers;
using Application.Commands.Users.UpdateUser;
using Application.Queries.Users.GetUsersByIds;
using Application.Commands.Users.FollowUser;

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
        public async Task<ActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users.ToList());
        }

        [SwaggerOperation(Summary = "Retrieves specific followers by given ids")]
        [HttpGet("followers")]
        public async Task<ActionResult> GetUsersByIds([FromQuery] List<int> ids)
        {
            var users = await _mediator.Send(new GetUsersByIdsQuery() {UserIds = ids});
            return Ok(users.ToList());
        }
        

        [SwaggerOperation(Summary = "Retrieves a specific user by username")]
        [HttpGet("{username}")]
        public async Task<ActionResult> GetUser(string username)
        {
            var user = await _mediator.Send(new GetUserQuery() {Username = username});
            return Ok(user);
        }

        [SwaggerOperation(Summary = "Updates a specific user")]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto) 
        {
            await _mediator.Send(new UpdateUserCommand() {UserUpdateDto = userUpdateDto});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Follows or unfollows a specific user")]
        [HttpPut("followers/{id}")]
        public async Task<ActionResult> FollowUser(int UserId, int id) 
        {
            await _mediator.Send(new FollowUserCommand() {UserId = UserId, FollowUserId = id});
            return NoContent();
        }
    }
}