using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.LikePost
{
    public record LikePostCommand(int PostId) : IRequest<ErrorOr<Unit>>;
}
