using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using Domain.Factories;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.LikeComment
{
    public class LikeCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUserService currentUserService) : IRequestHandler<LikeCommentCommand, ErrorOr<Unit>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
            if (comment is null)
                return Errors.Comment.NotFound;

            var userId = _currentUserService.UserId;
            if (comment.Likes.Any(l => l.UserId == userId))
                return Errors.Comment.AlreadyLiked;

            var like = CommentLikeFactory.Create(userId, comment.Id);
            await _commentRepository.AddLikeAsync(like, cancellationToken);
            return Unit.Value;
        }
    }
}
