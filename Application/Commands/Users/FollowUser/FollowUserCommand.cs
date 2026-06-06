using MediatR;

namespace Application.Commands.Users.FollowUser
{
    public class FollowUserCommand : IRequest
    {
        public int UserId { get; set; }
        public int FollowUserId { get; set; }
    }
}