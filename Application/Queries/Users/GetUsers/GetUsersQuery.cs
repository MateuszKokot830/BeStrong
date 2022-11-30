using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsers
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {      
    }
}