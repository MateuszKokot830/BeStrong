using MediatR;

namespace Application.Commands.Users.FollowUser
{
    public record FollowUserCommand(int UserId, int FollowUserId) : IRequest;
}