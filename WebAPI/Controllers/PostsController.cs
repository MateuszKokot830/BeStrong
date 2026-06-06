using Application.Commands.Posts.CreateComment;
using Application.Commands.Posts.CreatePost;
using Application.Commands.Posts.DeleteComment;
using Application.Commands.Posts.DeletePost;
using Application.Dto.Comment;
using Application.Dto.Post;
using Application.Queries.Posts.GetFollowedUsersPosts;
using Application.Queries.Posts.GetPosts;
using Application.Queries.Posts.GetUserPosts;
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
        public async Task<ActionResult> CreatePost(PostCreateDto postCreateDto)
        {
            await _mediator.Send(new CreatePostCommand() {PostCreateDto = postCreateDto});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<ActionResult> GetPosts()
        {
            var posts = await _mediator.Send(new GetPostsQuery());
            return Ok(posts.ToList());
        }

        [SwaggerOperation(Summary = "Retrieves all posts from a specific user")]
        [HttpGet("users/{userId}")]
        public async Task<ActionResult> GetUserPosts(int userId)
        {
            var posts = await _mediator.Send(new GetUserPostsQuery() {UserId = userId});
            return Ok(posts.ToList());
        }

        [SwaggerOperation(Summary = "Retrieves all posts from specific followers by given ids")]
        [HttpGet("users/followers")]
        public async Task<ActionResult> GetUserPosts([FromQuery] List<int> ids)
        {
            var posts = await _mediator.Send(new GetFollowedUsersPostsQuery() {FollowersIds = ids});
            return Ok(posts.ToList());
        }

        [SwaggerOperation(Summary = "Creates a comment to post")]
        [HttpPost("comments")]
        public async Task<ActionResult> CreateComment(CommentCreateDto commentCreateDto)
        {
            await _mediator.Send(new CreateCommentCommand () {CommentCreateDto = commentCreateDto});
            return NoContent();
        }

        [SwaggerOperation(Summary = "Deletes a post by id")]
        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePost(int postId)
        {
            await _mediator.Send(new DeletePostCommand() { PostId = postId });
            return NoContent();
        }

        [SwaggerOperation(Summary = "Deletes a comment by id")]
        [HttpDelete("comments/{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            await _mediator.Send(new DeleteCommentCommand() { CommentId = commentId });
            return NoContent();
        }
    }
}