using Application.Dto.User;
using Application.Helpers;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public record GetUsersListQuery(PaginationParams PaginationParams) : IRequest<PaginationList<UserDto>>;
}