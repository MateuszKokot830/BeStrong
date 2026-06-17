using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeletePost
{
    public record DeletePostCommand(int PostId) : IRequest<ErrorOr<Unit>>;
}
