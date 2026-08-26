using Application.Dto.User;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserSettings
{
    public class GetUserSettingsQueryHandler(
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetUserSettingsQuery, ErrorOr<UserSettingsDto>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<UserSettingsDto>> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
        {
            return await _userSearcher.GetSettingsAsync(_currentUserService.UserId, cancellationToken);
        }
    }
}
