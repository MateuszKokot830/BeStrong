using Application.Commands.Workouts.CreateWorkout;
using Application.Validators.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
    {
        public CreateWorkoutCommandValidator()
        {
            RuleFor(x => x.WorkoutDto.Name)
                .NotEmpty().WithMessage("Workout name is required.")
                .MaximumLength(100).WithMessage("Workout name cannot exceed 100 characters.");

            RuleFor(x => x.WorkoutDto.Exercises)
                .NotEmpty().WithMessage("A workout must contain at least one exercise.");

            RuleForEach(x => x.WorkoutDto.Exercises)
                .SetValidator(new WorkoutExerciseDtoValidator());
        }
    }
}
