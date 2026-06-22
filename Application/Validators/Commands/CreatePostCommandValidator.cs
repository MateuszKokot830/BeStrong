using Application.Commands.Posts.CreatePost;
using Domain.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.PostCreateDto.Description)
                .MaximumLength(2000).WithMessage("Post description cannot exceed 2000 characters.")
                .When(x => x.PostCreateDto.Description is not null);

            RuleFor(x => x.PostCreateDto.WorkoutId)
                .NotNull().WithMessage("A workout publication must reference a workout.")
                .When(x => x.PostCreateDto.Type == PostType.WorkoutPublication);

            RuleFor(x => x.PostCreateDto.Description)
                .NotNull().WithMessage("A normal post must have a description.")
                .When(x => x.PostCreateDto.Type == PostType.Normal);
        }
    }
}
