using Application.Dto.User;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUsersQuery, ErrorOr<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.ProjectAsync(UserMappings.Selector, cancellationToken);
            return users.OrderBy(u => u.UserName).ToList();
        }
    }
}
