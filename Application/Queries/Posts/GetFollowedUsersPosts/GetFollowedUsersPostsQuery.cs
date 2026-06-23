using Application.Dto.Post;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public record GetFollowedUsersPostsQuery : IRequest<ErrorOr<IEnumerable<PostDto>>>;
}
