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
using Application.Commands.Users.AddPhoto;
using WebAPI.Extensions;
using Application.Commands.Users.SetMainPhoto;
using Application.Commands.Users.DeletePhoto;
using Application.Helpers;
using Application.Queries.Users.GetUsersList;

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

        [SwaggerOperation(Summary = "Retrieves all users as pagination list")]
        [HttpGet("list")]
        public async Task<ActionResult> GetUsersList([FromQuery]PaginationParams paginationParams)
        {
            var users = await _mediator.Send(new GetUsersListQuery() { PaginationParams = paginationParams });
            Response.AddPaginationHeader(new PaginationHeader(
                users.CurrentPage, 
                users.PageSize,
                users.TotalItems,
                users.TotalPages));
            return Ok(users);
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
        public async Task<ActionResult> FollowUser(int userId, int id) 
        {
            await _mediator.Send(new FollowUserCommand() {UserId = userId, FollowUserId = id});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Adds a photo assigned to current logged user")]
        [HttpPut("{userId}/photos")]
        public async Task<ActionResult> AddPhoto(IFormFile file, int userId) 
        {
            await _mediator.Send(new AddPhotoCommand() {File = file, UserId = userId});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Sets a photo as a main photo to current logged user")]
        [HttpPut("{userId}/photos/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId, int userId) 
        {
            await _mediator.Send(new SetMainPhotoCommand() {PhotoId = photoId, UserId = userId});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Deletes a photo from current logged user")]
        [HttpDelete("{userId}/photos/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId, int userId) 
        {
            await _mediator.Send(new DeletePhotoCommand() {PhotoId = photoId, UserId = userId});
            return NoContent();
        }
    }
}