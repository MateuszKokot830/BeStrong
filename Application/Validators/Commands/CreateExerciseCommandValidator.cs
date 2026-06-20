using Application.Commands.Workouts.CreateExercise;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
    {
        public CreateExerciseCommandValidator()
        {
            RuleFor(x => x.ExerciseDto.Name)
                .NotEmpty().WithMessage("Exercise name is required.")
                .MaximumLength(100).WithMessage("Exercise name cannot exceed 100 characters.");

            RuleFor(x => x.ExerciseDto.Description)
                .MaximumLength(500).WithMessage("Exercise description cannot exceed 500 characters.")
                .When(x => x.ExerciseDto.Description is not null);

            RuleFor(x => x.ExerciseDto.ImageUrl)
                .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.")
                .When(x => x.ExerciseDto.ImageUrl is not null);
        }
    }
}
