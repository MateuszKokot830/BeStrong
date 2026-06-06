using Application.Dto.User;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public required UserUpdateDto UserUpdateDto { get; set; }
    }
}