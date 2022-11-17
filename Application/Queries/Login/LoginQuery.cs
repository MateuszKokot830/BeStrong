using Application.Dto;
using ErrorOr;
using MediatR;

namespace Application.Queries.Login
{
    public class LoginQuery : IRequest<ErrorOr<UserAuthResponseDto>>
    {
        public UserLoginRequestDto userLoginRequestDto { get; set; }
        public UserAggregateDto userAggregateDto { get; set; }
    }
}