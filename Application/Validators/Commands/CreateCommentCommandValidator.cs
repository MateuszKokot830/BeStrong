using Application.Commands.Posts.CreateComment;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.CommentCreateDto.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            RuleFor(x => x.CommentCreateDto.PostId)
                .GreaterThan(0).WithMessage("PostId must be a valid positive integer.");

            RuleFor(x => x.CommentCreateDto.Description)
                .NotEmpty().WithMessage("Comment description is required.")
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}
