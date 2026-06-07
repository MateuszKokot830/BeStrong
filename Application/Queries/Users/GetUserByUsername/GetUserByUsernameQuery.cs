using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserByUsername
{
    public record GetUserByUsernameQuery(string Username) : IRequest<ErrorOr<UserDto>>;
}
