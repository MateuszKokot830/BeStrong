using Application.Dto.WorkoutPlan;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutTemplateExerciseDtoValidator : AbstractValidator<WorkoutTemplateExerciseDto>
    {
        public WorkoutTemplateExerciseDtoValidator()
        {
            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order cannot be negative.");

            RuleFor(x => x.ExerciseId)
                .GreaterThan(0).WithMessage("ExerciseId must be a valid positive integer.");
        }
    }
}
