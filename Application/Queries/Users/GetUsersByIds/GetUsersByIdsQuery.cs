using Application.Dto.User;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public record GetUsersByIdsQuery(IReadOnlyCollection<int> UserIds) : IRequest<ErrorOr<IEnumerable<UserDto>>>;
}
