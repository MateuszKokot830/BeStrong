using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Posts.GetUserPosts
{
    public class GetUserPostsQueryHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<GetUserPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PostDto>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllUserPostsAsync(request.UserId);
            posts?.OrderBy(a => a.CreatedDate);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}