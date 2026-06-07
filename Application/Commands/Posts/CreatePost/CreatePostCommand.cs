using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public record CreatePostCommand(PostCreateDto PostCreateDto) : IRequest<ErrorOr<PostDto>>;
}