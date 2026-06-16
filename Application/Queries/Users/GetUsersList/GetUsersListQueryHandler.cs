using Application.Dto.User;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUsersListQuery, ErrorOr<PaginationList<UserDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<PaginationList<UserDto>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var p = request.PaginationParams;

            return await _userRepository.GetPagedAsync(
                UserMappings.Selector,
                u => u.UserName != p.Username,
                p.PageNumber,
                p.PageSize,
                cancellationToken);
        }
    }
}
