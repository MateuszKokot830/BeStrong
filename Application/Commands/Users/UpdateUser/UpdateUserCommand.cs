using Application.Dto;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public UserUpdateDto UserUpdateDto { get; set; }
    }
}