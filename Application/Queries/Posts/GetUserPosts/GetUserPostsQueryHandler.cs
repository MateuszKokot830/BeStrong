using Application.Dto.Post;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public class GetUserPostsQueryHandler(IPostSearcher postSearcher)
        : IRequestHandler<GetUserPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostSearcher _postSearcher = postSearcher;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postSearcher.FindByUserIdAsync(request.UserId, cancellationToken);
            return posts.ToList();
        }
    }
}
