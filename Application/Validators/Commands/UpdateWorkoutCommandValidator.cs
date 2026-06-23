using Application.Commands.Workouts.UpdateWorkout;
using Application.Validators.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UpdateWorkoutCommandValidator : AbstractValidator<UpdateWorkoutCommand>
    {
        public UpdateWorkoutCommandValidator()
        {
            RuleFor(x => x.WorkoutId)
                .GreaterThan(0).WithMessage("WorkoutId must be a valid positive integer.");

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
