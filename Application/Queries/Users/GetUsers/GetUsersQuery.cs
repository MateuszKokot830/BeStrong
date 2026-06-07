using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public record GetUsersQuery : IRequest<ErrorOr<IEnumerable<UserDto>>>;
}
