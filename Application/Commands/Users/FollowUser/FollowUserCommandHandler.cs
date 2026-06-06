using MediatR;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Commands.Users.FollowUser
{
    public class FollowUserCommandHandler(IUserRepository userRepository) : IRequestHandler<FollowUserCommand>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var followUser = await _userRepository.GetByIdAsync(request.FollowUserId);

            if (user == null || followUser == null || user.FollowedUsers == null)
                return Unit.Value;

            var isFollowed = user.FollowedUsers.FirstOrDefault(x => x.FollowedUserId == followUser.Id);

            if (isFollowed != null)
            {
                await _userRepository.DeleteFollowerAsync(isFollowed);
            }
            else
            {
                var follower = new Follower
                {
                    UserId = user.Id,
                    User = user,
                    FollowedUserId = followUser.Id,
                    FollowedUser = followUser
                };
                await _userRepository.AddFollowerAsync(follower);
            }

            return Unit.Value;
        }
    }
}