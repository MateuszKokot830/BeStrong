using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public class GetUserPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        : IRequestHandler<GetUserPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllUserPostsAsync(request.UserId, cancellationToken);
            return _mapper.Map<IEnumerable<PostDto>>(posts).OrderBy(p => p.CreatedDate).ToList();
        }
    }
}
