using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUserService currentUserService) : IRequestHandler<DeleteCommentCommand, ErrorOr<Unit>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
            if (comment is null)
                return Errors.Comment.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(comment.UserId))
                return Errors.Comment.Unauthorized;

            await _commentRepository.DeleteAsync(comment, cancellationToken);
            return Unit.Value;
        }
    }
}
