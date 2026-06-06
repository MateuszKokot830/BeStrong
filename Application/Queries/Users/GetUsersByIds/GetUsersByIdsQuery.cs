using Application.Dto.User;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public record GetUsersByIdsQuery(IReadOnlyCollection<int> UserIds) : IRequest<IEnumerable<UserDto>>;
}