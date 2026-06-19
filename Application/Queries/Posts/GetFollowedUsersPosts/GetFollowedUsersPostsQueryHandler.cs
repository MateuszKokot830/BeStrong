using Application.Dto.Post;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandler(IPostSearcher postSearcher)
        : IRequestHandler<GetFollowedUsersPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostSearcher _postSearcher = postSearcher;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetFollowedUsersPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postSearcher.FindByUserIdsAsync(request.FollowersIds, cancellationToken);
            return posts.ToList();
        }
    }
}
