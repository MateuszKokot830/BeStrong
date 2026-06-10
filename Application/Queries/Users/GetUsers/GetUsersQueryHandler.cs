using Application.Dto.User;
using Application.Interfaces.Repositories;
using Domain.Errors;
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
            try
            {
                var users = await _userRepository.ProjectAsync(UserDto.Selector, cancellationToken);
                return users.OrderBy(u => u.UserName).ToList();
            }
            catch (Exception)
            {
                return Errors.User.NotFound;
            }
        }
    }
}
