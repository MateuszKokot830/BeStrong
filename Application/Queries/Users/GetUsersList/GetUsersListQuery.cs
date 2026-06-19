using Application.Dto.User;
using Application.Helpers;
using Application.Helpers.Criteria;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersList
{
    public record GetUsersListQuery(UserSearchCriteria Criteria) : IRequest<ErrorOr<PaginationList<UserDto>>>;
}
