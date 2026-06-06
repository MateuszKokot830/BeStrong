using Application.Dto.User;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}