using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Commands.Register
{
    public class RegisterCommand : IRequest<ErrorOr<UserAuthResponseDto>>
    {
        public UserRegisterRequestDto userRegisterRequestDto { get; set; }
    }
}