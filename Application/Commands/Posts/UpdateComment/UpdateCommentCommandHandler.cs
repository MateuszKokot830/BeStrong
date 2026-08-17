using Application.Dto.Comment;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.UpdateComment
{
    public class UpdateCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateCommentCommand, ErrorOr<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ErrorOr<CommentDto>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
            if (comment is null)
                return Errors.Comment.NotFound;

            if (!_currentUserService.IsOwnerOrAdmin(comment.UserId))
                return Errors.Comment.Forbidden;

            comment.Description = request.UpdateCommentDto.Description;

            await _commentRepository.UpdateAsync(comment, cancellationToken);
            return comment.ToDto();
        }
    }
}
