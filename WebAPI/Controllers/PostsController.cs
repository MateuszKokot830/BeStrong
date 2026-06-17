using Application.Commands.Posts.CreateComment;
using Application.Commands.Posts.CreatePost;
using Application.Commands.Posts.DeleteComment;
using Application.Commands.Posts.DeletePost;
using Application.Dto.Comment;
using Application.Dto.Post;
using Application.Queries.Posts.GetFollowedUsersPosts;
using Application.Queries.Posts.GetPosts;
using Application.Queries.Posts.GetUserPosts;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class PostsController(IMediator mediator) : BaseApiController
    {
        private readonly IMediator _mediator = mediator;

        [SwaggerOperation(Summary = "Creates a new post")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreateDto postCreateDto)
        {
            ErrorOr<PostDto> result = await _mediator.Send(new CreatePostCommand(postCreateDto));
            
            return result.Match(
                post => Ok(post),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            ErrorOr<IEnumerable<PostDto>> result = await _mediator.Send(new GetPostsQuery());
            
            return result.Match(
                posts => Ok(posts),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all posts from a specific user")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserPosts(int userId)
        {
            ErrorOr<IEnumerable<PostDto>> result = await _mediator.Send(new GetUserPostsQuery(userId));
            
            return result.Match(
                posts => Ok(posts),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves all posts from specific followers by given ids")]
        [HttpGet("users/followers")]
        public async Task<IActionResult> GetFollowedUsersPosts([FromQuery] List<int> ids)
        {
            ErrorOr<IEnumerable<PostDto>> result = await _mediator.Send(new GetFollowedUsersPostsQuery(ids));
            
            return result.Match(
                posts => Ok(posts),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Creates a comment to post")]
        [HttpPost("comments")]
        public async Task<IActionResult> CreateComment(CommentCreateDto commentCreateDto)
        {
            ErrorOr<CommentDto> result = await _mediator.Send(new CreateCommentCommand(commentCreateDto));
            
            return result.Match(
                comment => Ok(comment),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Deletes a post by id")]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new DeletePostCommand(postId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Deletes a comment by id")]
        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new DeleteCommentCommand(commentId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }
    }
}