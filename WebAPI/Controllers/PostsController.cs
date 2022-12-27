using Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Commands.Posts.CreatePost;
using Application.Queries.Posts.GetPosts;
using Application.Commands.Posts.CreateComment;
using Application.Queries.Posts.GetUserPosts;
using Application.Queries.Posts.GetFollowedUsersPosts;

namespace WebAPI.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
        [HttpGet("users/{id}")]
        public async Task<ActionResult> GetUserPosts(int id)
        {
            var posts = await _mediator.Send(new GetUserPostsQuery() {UserId = id});
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
    }
}