using Application.Dto.Post;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommand : IRequest
    {
        public required PostCreateDto PostCreateDto { get; set; }
    }
}