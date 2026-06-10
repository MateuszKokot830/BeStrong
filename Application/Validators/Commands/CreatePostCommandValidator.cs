using Application.Commands.Posts.CreatePost;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.PostCreateDto.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");

            RuleFor(x => x.PostCreateDto.Description)
                .MaximumLength(2000).WithMessage("Post description cannot exceed 2000 characters.")
                .When(x => x.PostCreateDto.Description is not null);

            RuleFor(x => x.PostCreateDto)
                .Must(dto => dto.Description is not null || dto.WorkoutId.HasValue || dto.WorkoutPlan.HasValue)
                .WithMessage("A post must have a description, a linked workout, or a linked workout plan.");
        }
    }
}
