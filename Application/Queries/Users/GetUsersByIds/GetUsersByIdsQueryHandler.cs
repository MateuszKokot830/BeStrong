using Application.Dto.User;
using Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public class GetUsersByIdsQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUsersByIdsQuery, ErrorOr<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<IEnumerable<UserDto>>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.ProjectAsync(
                UserDto.Selector,
                u => request.UserIds.Contains(u.Id),
                cancellationToken);

            return users.ToList();
        }
    }
}
