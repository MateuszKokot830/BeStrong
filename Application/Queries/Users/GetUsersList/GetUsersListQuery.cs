using Application.Dto.User;
using Application.Helpers;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public class GetUsersListQuery : IRequest<PaginationList<UserDto>>
    {
        public required PaginationParams PaginationParams { get; set; }
    }
}