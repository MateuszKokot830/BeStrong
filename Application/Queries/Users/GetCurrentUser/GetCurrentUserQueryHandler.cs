using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetCurrentUser
{
    public class GetCurrentUserQueryHandler(
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetCurrentUserQuery, ErrorOr<UserDto>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userSearcher.FindByIdAsync(_currentUserService.UserId, cancellationToken);
            if (user is null)
                return Errors.User.NotFound;

            return user;
        }
    }
}
