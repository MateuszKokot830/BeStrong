using Application.Dto.Post;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public record CreatePostCommand(PostCreateDto PostCreateDto) : IRequest;
}