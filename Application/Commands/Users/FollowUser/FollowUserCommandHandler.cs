using MediatR;
using Domain.Entities;
using Domain.Errors;
using ErrorOr;
using Application.Interfaces.Repositories;

namespace Application.Commands.Users.FollowUser
{
    public class FollowUserCommandHandler(IUserRepository userRepository) : IRequestHandler<FollowUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<Unit>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            var followUser = await _userRepository.GetByIdAsync(request.FollowUserId, cancellationToken);

            if (user is null || followUser is null)
                return Errors.User.NotFound;

            if (user.FollowedUsers is null)
                return Unit.Value;

            var isFollowed = user.FollowedUsers.FirstOrDefault(x => x.FollowedUserId == followUser.Id);

            if (isFollowed != null)
            {
                await _userRepository.DeleteFollowerAsync(isFollowed, cancellationToken);
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
                await _userRepository.AddFollowerAsync(follower, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
