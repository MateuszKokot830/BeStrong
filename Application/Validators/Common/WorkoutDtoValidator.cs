using Application.Dto.Workout;
using FluentValidation;

namespace Application.Validators.Common
{
    public sealed class WorkoutDtoValidator : AbstractValidator<WorkoutDto>
    {
        public WorkoutDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workout name is required.")
                .MaximumLength(100).WithMessage("Workout name cannot exceed 100 characters.");

            RuleFor(x => x.WorkoutExercises)
                .NotEmpty().WithMessage("A workout must contain at least one exercise.");

            RuleForEach(x => x.WorkoutExercises)
                .SetValidator(new WorkoutExerciseDtoValidator());
        }
    }
}
