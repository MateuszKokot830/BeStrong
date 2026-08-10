using Application.Dto.Post;
using Application.Interfaces.Common;
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
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork) : IRequestHandler<CreatePostCommand, ErrorOr<PostDto>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PostCreateDto;
            var post = PostFactory.Create(_currentUserService.UserId, dto.Type, dto.Description, dto.WorkoutId, dto.WorkoutPlan);
            await _postRepository.AddAsync(post, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return post.ToDto();
        }
    }
}
