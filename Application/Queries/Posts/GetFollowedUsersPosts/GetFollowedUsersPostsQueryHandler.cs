using Application.Dto.Post;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Queries.Posts.GetFollowedUsersPosts
{
    public class GetFollowedUsersPostsQueryHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<GetFollowedUsersPostsQuery, ErrorOr<IEnumerable<PostDto>>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ErrorOr<IEnumerable<PostDto>>> Handle(GetFollowedUsersPostsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var posts = await _postRepository.GetAllFollowedUsersPostsAsync([.. request.FollowersIds], cancellationToken);
                return _mapper.Map<IEnumerable<PostDto>>(posts).ToList();
            }
            catch (Exception)
            {
                return Errors.Post.NotFound;
            }
        }
    }
}
