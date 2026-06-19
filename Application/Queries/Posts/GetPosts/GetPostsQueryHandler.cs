using Application.Dto.Post;
using Application.Interfaces.Searchers;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public class GetPostsQueryHandler(IPostSearcher postSearcher)
        : IRequestHandler<GetPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostSearcher _postSearcher = postSearcher;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postSearcher.GetAllAsync(cancellationToken);
            return posts.ToList();
        }
    }
}
