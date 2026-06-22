using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using Domain.Factories;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.LikePost
{
    public class LikePostCommandHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService) : IRequestHandler<LikePostCommand, ErrorOr<Unit>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
                return Errors.Post.NotFound;

            var userId = _currentUserService.UserId;
            if (post.Likes.Any(l => l.UserId == userId))
                return Errors.Post.AlreadyLiked;

            var like = PostLikeFactory.Create(userId, post.Id);
            await _postRepository.AddLikeAsync(like, cancellationToken);
            return Unit.Value;
        }
    }
}
