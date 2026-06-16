using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public class GetPostsQueryHandler(IPostRepository postRepository)
        : IRequestHandler<GetPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllAsync(cancellationToken);
            return posts.Select(p => p.ToDto()).OrderBy(p => p.CreatedDate).ToList();
        }
    }
}
