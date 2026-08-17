using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using Domain.Factories;
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
                return Errors.User.Forbidden;

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            var followUser = await _userRepository.GetByIdAsync(request.FollowUserId, cancellationToken);

            if (user is null || followUser is null)
                return Errors.User.NotFound;

            var alreadyFollowing = user.FollowedUsers.Any(x => x.FollowedUserId == followUser.Id);
            if (alreadyFollowing)
                return Unit.Value;

            var follower = FollowerFactory.Create(user, followUser);

            await _userRepository.AddFollowerAsync(follower, cancellationToken);
            return Unit.Value;
        }
    }
}
