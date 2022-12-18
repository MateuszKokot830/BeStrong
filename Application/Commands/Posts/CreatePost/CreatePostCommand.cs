using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommand : IRequest
    {
        public PostCreateDto PostCreateDto { get; set; }
    }
}