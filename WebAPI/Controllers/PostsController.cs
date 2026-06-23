using Application.Commands.Posts.CreateComment;
using Application.Commands.Posts.CreatePost;
using Application.Commands.Posts.DeleteComment;
using Application.Commands.Posts.DeletePost;
using Application.Commands.Posts.LikeComment;
using Application.Commands.Posts.LikePost;
using Application.Commands.Posts.UnlikeComment;
using Application.Commands.Posts.UnlikePost;
using Application.Commands.Posts.UpdateComment;
using Application.Commands.Posts.UpdatePost;
using Application.Dto.Comment;
using Application.Dto.Post;
using Application.Queries.Posts.GetFollowedUsersPosts;
using Application.Queries.Posts.GetPostById;
using Application.Queries.Posts.GetPosts;
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

        [SwaggerOperation(Summary = "Retrieves posts from users the current user follows")]
        [HttpGet("feed")]
        public async Task<IActionResult> GetFollowedUsersPosts()
        {
            ErrorOr<IEnumerable<PostDto>> result = await _mediator.Send(new GetFollowedUsersPostsQuery());

            return result.Match(
                posts => Ok(posts),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Retrieves a single post by id")]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            ErrorOr<PostDto> result = await _mediator.Send(new GetPostByIdQuery(postId));

            return result.Match(
                post => Ok(post),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Updates a post by id")]
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, UpdatePostDto updatePostDto)
        {
            ErrorOr<PostDto> result = await _mediator.Send(new UpdatePostCommand(postId, updatePostDto));

            return result.Match(
                post => Ok(post),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Updates a comment by id")]
        [HttpPut("comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, UpdateCommentDto updateCommentDto)
        {
            ErrorOr<CommentDto> result = await _mediator.Send(new UpdateCommentCommand(commentId, updateCommentDto));

            return result.Match(
                comment => Ok(comment),
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

        [SwaggerOperation(Summary = "Likes a post")]
        [HttpPost("{postId}/like")]
        public async Task<IActionResult> LikePost(int postId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new LikePostCommand(postId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Unlikes a post")]
        [HttpDelete("{postId}/like")]
        public async Task<IActionResult> UnlikePost(int postId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new UnlikePostCommand(postId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Likes a comment")]
        [HttpPost("comments/{commentId}/like")]
        public async Task<IActionResult> LikeComment(int commentId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new LikeCommentCommand(commentId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [SwaggerOperation(Summary = "Unlikes a comment")]
        [HttpDelete("comments/{commentId}/like")]
        public async Task<IActionResult> UnlikeComment(int commentId)
        {
            ErrorOr<Unit> result = await _mediator.Send(new UnlikeCommentCommand(commentId));

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }
    }
}