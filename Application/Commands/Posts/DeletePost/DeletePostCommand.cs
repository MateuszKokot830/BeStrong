using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeletePost
{
    public record DeletePostCommand(int PostId, int UserId) : IRequest<ErrorOr<Unit>>;
}