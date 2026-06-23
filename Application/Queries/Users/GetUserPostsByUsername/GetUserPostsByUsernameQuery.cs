using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Queries.Users.GetUserPostsByUsername
{
    public record GetUserPostsByUsernameQuery(string Username) : IRequest<ErrorOr<IEnumerable<PostDto>>>;
}
