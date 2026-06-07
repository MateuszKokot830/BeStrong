using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public record GetPostsQuery : IRequest<ErrorOr<IEnumerable<PostDto>>>;
}
