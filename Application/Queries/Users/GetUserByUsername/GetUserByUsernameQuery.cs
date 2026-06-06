using Application.Dto.User;
using MediatR;

namespace Application.Queries.Users.GetUserByUsername
{
    public record GetUserByUsernameQuery(string Username) : IRequest<UserDto>;
}