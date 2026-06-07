using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public record UpdateUserCommand(UserUpdateDto UserUpdateDto) : IRequest<ErrorOr<UserDto>>;
}
