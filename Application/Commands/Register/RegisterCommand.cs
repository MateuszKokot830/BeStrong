using Application.Dto.Auth;
using ErrorOr;
using MediatR;

namespace Application.Commands.Register
{
    public class RegisterCommand : IRequest<ErrorOr<UserAuthResponseDto>>
    {
        public required UserRegisterRequestDto UserRegisterRequestDto { get; set; }
    }
}