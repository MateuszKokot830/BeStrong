using Application.Commands.Workouts.CreateWorkout;
using Application.Validators.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
    {
        public CreateWorkoutCommandValidator()
        {
            RuleFor(x => x.WorkoutDto.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.")
                .When(x => x.WorkoutDto.UserId.HasValue);

            RuleFor(x => x.WorkoutDto.Name)
                .NotEmpty().WithMessage("Workout name is required.")
                .MaximumLength(100).WithMessage("Workout name cannot exceed 100 characters.");

            RuleFor(x => x.WorkoutDto.WorkoutExercises)
                .NotEmpty().WithMessage("A workout must contain at least one exercise.");

            RuleForEach(x => x.WorkoutDto.WorkoutExercises)
                .SetValidator(new WorkoutExerciseDtoValidator());
        }
    }
}
