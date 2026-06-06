using Application.Dto.Auth;
using ErrorOr;
using MediatR;

namespace Application.Commands.Register
{
    public record RegisterCommand(UserRegisterRequestDto UserRegisterRequestDto) : IRequest<ErrorOr<UserAuthResponseDto>>;
}