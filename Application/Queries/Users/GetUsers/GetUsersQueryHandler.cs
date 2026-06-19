using Application.Dto.User;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQueryHandler(IUserSearcher userSearcher)
        : IRequestHandler<GetUsersQuery, ErrorOr<IEnumerable<UserDto>>>
    {
        private readonly IUserSearcher _userSearcher = userSearcher;

        public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userSearcher.GetAllAsync(cancellationToken);
            return users.ToList();
        }
    }
}
