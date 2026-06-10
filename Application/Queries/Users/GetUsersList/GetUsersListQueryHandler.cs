using Application.Dto.User;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Domain.Errors;
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
            try
            {
                var paginationParams = request.PaginationParams;

                return await _userRepository.GetPagedAsync(
                    UserDto.Selector,
                    u => u.UserName != paginationParams.Username,
                    paginationParams.PageNumber,
                    paginationParams.PageSize,
                    cancellationToken);
            }
            catch (Exception)
            {
                return Errors.User.NotFound;
            }
        }
    }
}
