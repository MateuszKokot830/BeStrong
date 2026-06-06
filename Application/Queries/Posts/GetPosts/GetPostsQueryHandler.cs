using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public class GetPostsQueryHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllAsync();
            posts?.OrderBy(a => a.CreatedDate);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}