using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UpdatePost
{
    public record UpdatePostCommand(int PostId, UpdatePostDto UpdatePostDto) : IRequest<ErrorOr<PostDto>>;
}
