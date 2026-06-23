using Application.Dto.Post;
using Application.Interfaces.Searchers;
using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandler(
        IPostSearcher postSearcher,
        IUserSearcher userSearcher,
        ICurrentUserService currentUserService) : IRequestHandler<GetFollowedUsersPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostSearcher _postSearcher = postSearcher;
        private readonly IUserSearcher _userSearcher = userSearcher;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetFollowedUsersPostsQuery request, CancellationToken cancellationToken)
        {
            var followedUserIds = await _userSearcher.GetFollowedUserIdsAsync(_currentUserService.UserId, cancellationToken);
            var posts = await _postSearcher.FindByUserIdsAsync(followedUserIds, cancellationToken);
            return posts.ToList();
        }
    }
}
