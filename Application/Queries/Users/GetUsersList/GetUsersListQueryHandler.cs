using Application.Dto.User;
using Application.Helpers;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersListQuery, PaginationList<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<PaginationList<UserDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsersAsync(request.PaginationParams);
        }
    }
}