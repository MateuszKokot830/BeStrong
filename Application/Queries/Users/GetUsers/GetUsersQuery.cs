using Application.Dto;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {      
    }
}