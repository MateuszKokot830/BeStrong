using Application.Commands.Posts.UpdateComment;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("CommentId must be a valid positive integer.");

            RuleFor(x => x.UpdateCommentDto.Description)
                .NotEmpty().WithMessage("Comment description is required.")
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}
