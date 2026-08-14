using Application.Commands.WorkoutPlans.UpdateWorkoutPlan;
using Application.Validators.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class UpdateWorkoutPlanCommandValidator : AbstractValidator<UpdateWorkoutPlanCommand>
    {
        public UpdateWorkoutPlanCommandValidator()
        {
            RuleFor(x => x.PlanId)
                .GreaterThan(0).WithMessage("PlanId must be a valid positive integer.");

            RuleFor(x => x.WorkoutPlanDto.Name)
                .NotEmpty().WithMessage("Workout plan name is required.")
                .MaximumLength(100).WithMessage("Workout plan name cannot exceed 100 characters.");

            RuleFor(x => x.WorkoutPlanDto.Description)
                .MaximumLength(1000).WithMessage("Workout plan description cannot exceed 1000 characters.")
                .When(x => x.WorkoutPlanDto.Description is not null);

            RuleFor(x => x.WorkoutPlanDto.WorkoutTemplates)
                .NotEmpty().WithMessage("A workout plan must contain at least one workout template.");

            RuleForEach(x => x.WorkoutPlanDto.WorkoutTemplates)
                .SetValidator(new WorkoutTemplateCreateDtoValidator());
        }
    }
}
