using Application.Dto.Workout;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutExerciseDtoValidator : AbstractValidator<WorkoutExerciseDto>
    {
        public WorkoutExerciseDtoValidator()
        {
            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order cannot be negative.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
                .When(x => x.Notes is not null);

            RuleFor(x => x.ExerciseId)
                .GreaterThan(0).WithMessage("ExerciseId must be a valid positive integer.");

            RuleFor(x => x.Sets)
                .NotEmpty().WithMessage("An exercise must contain at least one set.");

            RuleForEach(x => x.Sets)
                .SetValidator(new WorkoutSetDtoValidator());
        }
    }
}
