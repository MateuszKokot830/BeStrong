using Application.Dto.User;
using Application.Helpers;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQueryHandler(IUserSearcher userSearcher)
        : IRequestHandler<GetUsersListQuery, ErrorOr<PaginationList<UserDto>>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;

        public async Task<ErrorOr<PaginationList<UserDto>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            return await _userSearcher.GetPagedAsync(request.Criteria, cancellationToken);
        }
    }
}
