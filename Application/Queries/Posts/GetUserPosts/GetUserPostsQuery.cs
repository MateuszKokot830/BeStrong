using Application.Dto.Post;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public record GetUserPostsQuery(int UserId) : IRequest<IEnumerable<PostDto>>;
}