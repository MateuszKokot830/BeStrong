using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using ErrorOr;
using Application.Dto.Photo;
using Application.Dto.User;
using Application.Helpers;
using Application.Helpers.Criteria;
using Application.Commands.Users.AddPhoto;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Application.Commands.Users.DeletePhoto;
using Application.Commands.Users.FollowUser;
using Application.Commands.Users.UnfollowUser;
using Application.Commands.Users.SetMainPhoto;
using Application.Commands.Users.UpdateUser;
using Application.Queries.Users.GetCurrentUser;
using Application.Queries.Users.GetUserByUsername;
using Application.Queries.Users.GetUserPostsByUsername;
using Application.Queries.Users.GetUsers;
using Application.Queries.Users.GetUsersByIds;
using Application.Queries.Users.GetUsersList;
using Application.Dto.Post;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    public class UsersController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Retrieves all users")]
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            ErrorOr<IEnumerable<UserDto>> result = await _mediator.Send(new GetUsersQuery());
            
            return result.Match(
                users => Ok(users),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all users as pagination list")]
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("list")]
        public async Task<IActionResult> GetUsersList([FromQuery] UserSearchCriteria criteria)
        {
            ErrorOr<PaginationList<UserDto>> result = await _mediator.Send(new GetUsersListQuery(criteria));
            
            if (result.IsError)
                return Problem(result.Errors);

            Response.AddPaginationHeader(new PaginationHeader(
                result.Value.CurrentPage,
                result.Value.PageSize,
                result.Value.TotalItems,
                result.Value.TotalPages));

            return Ok(result.Value);
        }

        [SwaggerOperation(Summary = "Retrieves specific followers by given ids")]
        [HttpGet("followers")]
        public async Task<IActionResult> GetUsersByIds([FromQuery] List<int> ids)
        {
            ErrorOr<IEnumerable<UserDto>> result = await _mediator.Send(new GetUsersByIdsQuery(ids));
            
            return result.Match(
                users => Ok(users),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves the current logged in user's profile")]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            ErrorOr<UserDto> result = await _mediator.Send(new GetCurrentUserQuery());

            return result.Match(
                user => Ok(user),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves a specific user by username")]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            ErrorOr<UserDto> result = await _mediator.Send(new GetUserByUsernameQuery(username));

            return result.Match(
                user => Ok(user),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all posts from a specific user by username")]
        [HttpGet("{username}/posts")]
        public async Task<IActionResult> GetUserPosts(string username)
        {
            ErrorOr<IEnumerable<PostDto>> result = await _mediator.Send(new GetUserPostsByUsernameQuery(username));

            return result.Match(
                posts => Ok(posts),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Updates a specific user")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            ErrorOr<UserDto> result = await _mediator.Send(new UpdateUserCommand(userUpdateDto));
            
            return result.Match(
                user => Ok(user),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Follows a specific user")]
        [HttpPost("{userId}/follow")]
        public async Task<IActionResult> FollowUser(int userId, [FromQuery] int followUserId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new FollowUserCommand(userId, followUserId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Unfollows a specific user")]
        [HttpDelete("{userId}/follow")]
        public async Task<IActionResult> UnfollowUser(int userId, [FromQuery] int unfollowUserId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new UnfollowUserCommand(userId, unfollowUserId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Adds a photo assigned to current logged user")]
        [HttpPut("{userId}/photos")]
        public async Task<IActionResult> AddPhoto(IFormFile file, int userId)
        {
            ErrorOr<PhotoDto> result = await _mediator.Send(new AddPhotoCommand(file, userId));
            
            return result.Match(
                photo => Ok(photo),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Sets a photo as a main photo to current logged user")]
        [HttpPut("{userId}/photos/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId, int userId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new SetMainPhotoCommand(photoId, userId));
            
            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Deletes a photo from current logged user")]
        [HttpDelete("{userId}/photos/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId, int userId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new DeletePhotoCommand(photoId, userId));
            
            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }
    }
}