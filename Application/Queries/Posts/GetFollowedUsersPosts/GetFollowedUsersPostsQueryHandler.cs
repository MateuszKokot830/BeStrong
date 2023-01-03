using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandler : IRequestHandler<GetFollowedUsersPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetFollowedUsersPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetFollowedUsersPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllFollowedUsersPostsAsync(request.FollowersIds);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}