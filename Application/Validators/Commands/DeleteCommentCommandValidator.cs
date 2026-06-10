using Application.Commands.Posts.DeleteComment;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("CommentId must be a valid positive integer.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");
        }
    }
}
