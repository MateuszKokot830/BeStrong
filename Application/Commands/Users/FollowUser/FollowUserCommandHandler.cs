using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.FollowUser
{
    public class FollowUserCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<FollowUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsOwnerOrAdmin(request.UserId))
                return Errors.User.Unauthorized;

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            var followUser = await _userRepository.GetByIdAsync(request.FollowUserId, cancellationToken);

            if (user is null || followUser is null)
                return Errors.User.NotFound;

            if (user.FollowedUsers is null)
                return Unit.Value;

            var existing = user.FollowedUsers.FirstOrDefault(x => x.FollowedUserId == followUser.Id);

            if (existing is not null)
            {
                await _userRepository.DeleteFollowerAsync(existing, cancellationToken);
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
