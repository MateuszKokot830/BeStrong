using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public record GetUserPostsQuery(int UserId) : IRequest<ErrorOr<IEnumerable<PostDto>>>;
}
