using Application.Dto.Post;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQuery : IRequest<IEnumerable<PostDto>>
    {
        public List<int> FollowersIds { get; set; } = [];
    }
}