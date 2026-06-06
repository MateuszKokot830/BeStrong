using Application.Dto.Post;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public record GetPostsQuery : IRequest<IEnumerable<PostDto>>;
}