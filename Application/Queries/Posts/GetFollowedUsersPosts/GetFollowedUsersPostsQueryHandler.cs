using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandler(IPostRepository postRepository)
        : IRequestHandler<GetFollowedUsersPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetFollowedUsersPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllFollowedUsersPostsAsync([.. request.FollowersIds], cancellationToken);
            return posts.Select(p => p.ToDto()).ToList();
        }
    }
}
