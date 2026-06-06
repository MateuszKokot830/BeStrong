using Application.Dto.Post;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public class GetPostsQuery : IRequest<IEnumerable<PostDto>>
    {
    }
}