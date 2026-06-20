using Application.Dto.Post;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Factories;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService) : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PostCreateDto;
            var post = PostFactory.Create(_currentUserService.UserId, dto.Description, dto.WorkoutId, dto.WorkoutPlan);
            await _postRepository.AddAsync(post, cancellationToken);
            return post.ToDto();
        }
    }
}
