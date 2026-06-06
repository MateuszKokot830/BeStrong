using Application.Dto.Auth;
using ErrorOr;
using MediatR;

namespace Application.Queries.Login
{
    public class LoginQuery : IRequest<ErrorOr<UserAuthResponseDto>>
    {
        public required UserLoginRequestDto userLoginRequestDto { get; set; }
    }
}