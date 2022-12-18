using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Commands.Posts.CreatePost;
using Application.Queries.Posts.GetPosts;

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
    }
}