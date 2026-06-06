using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<GetFollowedUsersPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PostDto>> Handle(GetFollowedUsersPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllFollowedUsersPostsAsync(request.FollowersIds);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}