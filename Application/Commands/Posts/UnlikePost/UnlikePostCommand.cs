using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UnlikePost
{
    public record UnlikePostCommand(int PostId) : IRequest<ErrorOr<Unit>>;
}
