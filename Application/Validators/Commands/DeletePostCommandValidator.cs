using Application.Commands.Posts.DeletePost;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("PostId must be a valid positive integer.");
        }
    }
}
