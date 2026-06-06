using Application.Dto.User;
using MediatR;

namespace Application.Queries.Users.GetUserByUsername
{
    public class GetUserByUsernameQuery : IRequest<UserDto>
    {
        public required string Username { get; set; }
    }
}