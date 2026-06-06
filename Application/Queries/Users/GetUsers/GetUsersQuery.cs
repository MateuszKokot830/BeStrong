using Application.Dto.User;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;
}