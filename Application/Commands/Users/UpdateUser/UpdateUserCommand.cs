using Application.Dto.User;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public record UpdateUserCommand(UserUpdateDto UserUpdateDto) : IRequest;
}