using Application.Dto.Auth;
using ErrorOr;
using MediatR;

namespace Application.Queries.Login
{
    public record LoginQuery(UserLoginRequestDto UserLoginRequestDto) : IRequest<ErrorOr<UserAuthResponseDto>>;
}