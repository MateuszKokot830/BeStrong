using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UnfollowUser
{
    public record UnfollowUserCommand(int UserId, int UnfollowUserId) : IRequest<ErrorOr<Unit>>;
}
