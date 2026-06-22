using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UnlikeComment
{
    public class UnlikeCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UnlikeCommentCommand, ErrorOr<Unit>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
            if (comment is null)
                return Errors.Comment.NotFound;

            var like = comment.Likes.FirstOrDefault(l => l.UserId == _currentUserService.UserId);
            if (like is null)
                return Unit.Value;

            await _commentRepository.DeleteLikeAsync(like, cancellationToken);
            return Unit.Value;
        }
    }
}
