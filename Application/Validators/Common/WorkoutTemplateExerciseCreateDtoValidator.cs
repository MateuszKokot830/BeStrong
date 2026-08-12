using Application.Dto.WorkoutPlan;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutTemplateExerciseCreateDtoValidator : AbstractValidator<WorkoutTemplateExerciseCreateDto>
    {
        public WorkoutTemplateExerciseCreateDtoValidator()
        {
            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order cannot be negative.");

            RuleFor(x => x.ExerciseId)
                .GreaterThan(0).WithMessage("ExerciseId must be a valid positive integer.");

            RuleFor(x => x.Sets)
                .GreaterThan(0).WithMessage("Sets must be greater than zero.");

            RuleFor(x => x.MinReps)
                .GreaterThan(0).WithMessage("MinReps must be greater than zero.");

            RuleFor(x => x.MaxReps)
                .GreaterThanOrEqualTo(x => x.MinReps).WithMessage("MaxReps cannot be less than MinReps.");
        }
    }
}
