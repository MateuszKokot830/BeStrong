using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetUserPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllUserPostsAsync(request.UserId);
            posts.OrderBy(a => a.CreatedDate);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}