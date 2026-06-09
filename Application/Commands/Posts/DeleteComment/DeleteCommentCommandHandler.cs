using Application.Interfaces.Repositories;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.DeleteComment
{
    public class DeleteCommentCommandHandler(IPostRepository postRepository) : IRequestHandler<DeleteCommentCommand, ErrorOr<Unit>>
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<ErrorOr<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _postRepository.GetCommentByIdAsync(request.CommentId, cancellationToken);

            if (comment is null)
                return Errors.Comment.NotFound;

            if (comment.UserId != request.UserId)
                return Errors.Comment.Unauthorized;

            await _postRepository.DeleteCommentAsync(comment, cancellationToken);

            return Unit.Value;
        }
    }
}
