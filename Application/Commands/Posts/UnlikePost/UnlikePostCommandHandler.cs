using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UnlikePost
{
    public class UnlikePostCommandHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UnlikePostCommand, ErrorOr<Unit>>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
                return Errors.Post.NotFound;

            var like = post.Likes.FirstOrDefault(l => l.UserId == _currentUserService.UserId);
            if (like is null)
                return Unit.Value;

            await _postRepository.DeleteLikeAsync(like, cancellationToken);
            return Unit.Value;
        }
    }
}
