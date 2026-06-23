using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetCurrentUser
{
    public record GetCurrentUserQuery : IRequest<ErrorOr<UserDto>>;
}
