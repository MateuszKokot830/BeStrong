using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Mappings;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public class GetUserPostsQueryHandler(IPostRepository postRepository)
        : IRequestHandler<GetUserPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllUserPostsAsync(request.UserId, cancellationToken);
            return posts.Select(p => p.ToDto()).OrderBy(p => p.CreatedDate).ToList();
        }
    }
}
