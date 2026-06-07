using Application.Dto.User;
using Application.Helpers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public record GetUsersListQuery(PaginationParams PaginationParams) : IRequest<ErrorOr<PaginationList<UserDto>>>;
}
