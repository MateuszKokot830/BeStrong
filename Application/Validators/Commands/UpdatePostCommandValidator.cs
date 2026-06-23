using Application.Commands.Posts.UpdatePost;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("PostId must be a valid positive integer.");

            RuleFor(x => x.UpdatePostDto.Description)
                .MaximumLength(2000).WithMessage("Post description cannot exceed 2000 characters.")
                .When(x => x.UpdatePostDto.Description is not null);
        }
    }
}
