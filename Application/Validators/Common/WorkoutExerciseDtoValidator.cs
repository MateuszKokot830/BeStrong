using Application.Dto.Workout;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutExerciseDtoValidator : AbstractValidator<WorkoutExerciseDto>
    {
        public WorkoutExerciseDtoValidator()
        {
            RuleFor(x => x.Sets)
                .GreaterThan(0).WithMessage("Sets must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("Sets cannot exceed 20.");

            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Reps must be greater than 0.")
                .LessThanOrEqualTo(1000).WithMessage("Reps cannot exceed 1000.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0.")
                .LessThanOrEqualTo(1000).WithMessage("Weight cannot exceed 1000 kg.")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.ExerciseId)
                .GreaterThan(0).WithMessage("ExerciseId must be a valid positive integer.");
        }
    }
}
