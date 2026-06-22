using Application.Dto.Workout;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutSetDtoValidator : AbstractValidator<WorkoutSetDto>
    {
        public WorkoutSetDtoValidator()
        {
            RuleFor(x => x.SetNumber)
                .GreaterThan(0).WithMessage("Set number must be greater than 0.");

            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Reps must be greater than 0.")
                .LessThanOrEqualTo(1000).WithMessage("Reps cannot exceed 1000.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0.")
                .LessThanOrEqualTo(1000).WithMessage("Weight cannot exceed 1000 kg.")
                .When(x => x.Weight.HasValue);
        }
    }
}
