using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetPosts
{
    public class GetPostsQueryHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<GetPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var posts = await _postRepository.GetAllAsync(cancellationToken);
                return _mapper.Map<IEnumerable<PostDto>>(posts).OrderBy(a => a.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return Errors.Post.NotFound;
            }
        }
    }
}
