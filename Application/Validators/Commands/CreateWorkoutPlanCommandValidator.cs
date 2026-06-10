using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Validators.Common;
using FluentValidation;

namespace Application.Validators.Commands
{
    public sealed class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
    {
        public CreateWorkoutPlanCommandValidator()
        {
            RuleFor(x => x.WorkoutPlanCreateDto.CreatedById)
                .GreaterThan(0).WithMessage("CreatedById must be a valid positive integer.");

            RuleFor(x => x.WorkoutPlanCreateDto.Name)
                .NotEmpty().WithMessage("Workout plan name is required.")
                .MaximumLength(100).WithMessage("Workout plan name cannot exceed 100 characters.");

            RuleFor(x => x.WorkoutPlanCreateDto.Description)
                .MaximumLength(1000).WithMessage("Workout plan description cannot exceed 1000 characters.")
                .When(x => x.WorkoutPlanCreateDto.Description is not null);

            RuleFor(x => x.WorkoutPlanCreateDto.Workouts)
                .NotEmpty().WithMessage("A workout plan must contain at least one workout.");

            RuleForEach(x => x.WorkoutPlanCreateDto.Workouts)
                .SetValidator(new WorkoutDtoValidator());
        }
    }
}
