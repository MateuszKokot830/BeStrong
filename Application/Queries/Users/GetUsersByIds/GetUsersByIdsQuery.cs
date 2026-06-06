using Application.Dto.User;
using MediatR;

namespace Application.Queries.Users.GetUsersByIds
{
    public class GetUsersByIdsQuery : IRequest<IEnumerable<UserDto>>
    {
        public List<int> UserIds { get; set; } = [];
    }
}