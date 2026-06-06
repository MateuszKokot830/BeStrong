using Application.Dto.Post;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public record GetFollowedUsersPostsQuery(IReadOnlyCollection<int> FollowersIds) : IRequest<IEnumerable<PostDto>>;
}