using Application.Dto.User;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public class GetUsersByIdsQueryHandler(IUserSearcher userSearcher)
        : IRequestHandler<GetUsersByIdsQuery, ErrorOr<IEnumerable<UserDto>>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;

        public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {
            var users = await _userSearcher.FindByIdsAsync(request.UserIds, cancellationToken);
            return users.ToList();
        }
    }
}
