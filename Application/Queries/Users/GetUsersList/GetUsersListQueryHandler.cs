using Application.Dto.User;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersListQuery, ErrorOr<PaginationList<UserDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ErrorOr<PaginationList<UserDto>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userRepository.GetUsersAsync(request.PaginationParams, cancellationToken);
            }
            catch (Exception)
            {
                return Errors.User.NotFound;
            }
        }
    }
}
