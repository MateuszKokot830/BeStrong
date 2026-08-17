using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UnfollowUser
{
    public class UnfollowUserCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UnfollowUserCommand, ErrorOr<Unit>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsOwnerOrAdmin(request.UserId))
                return Errors.User.Forbidden;

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            var follower = user.FollowedUsers.FirstOrDefault(x => x.FollowedUserId == request.UnfollowUserId);
            if (follower is null)
                return Unit.Value;

            await _userRepository.DeleteFollowerAsync(follower, cancellationToken);
            return Unit.Value;
        }
    }
}
