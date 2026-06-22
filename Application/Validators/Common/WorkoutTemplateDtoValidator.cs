using Application.Dto.WorkoutPlan;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutTemplateDtoValidator : AbstractValidator<WorkoutTemplateDto>
    {
        public WorkoutTemplateDtoValidator()
        {
            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order cannot be negative.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Workout template name cannot exceed 100 characters.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Exercises)
                .NotEmpty().WithMessage("A workout template must contain at least one exercise.");

            RuleForEach(x => x.Exercises)
                .SetValidator(new WorkoutTemplateExerciseDtoValidator());
        }
    }
}
