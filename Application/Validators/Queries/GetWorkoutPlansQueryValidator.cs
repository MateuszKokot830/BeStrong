using Application.Queries.WorkoutPlans.GetWorkoutPlans;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetWorkoutPlansQueryValidator : AbstractValidator<GetWorkoutPlansQuery>
    {
        public GetWorkoutPlansQueryValidator()
        {
            RuleFor(x => x.Criteria.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Criteria.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50.");

            RuleFor(x => x.Criteria.Name)
                .MaximumLength(100).WithMessage("Name filter cannot exceed 100 characters.")
                .When(x => x.Criteria.Name is not null);

            RuleFor(x => x.Criteria.OwnerName)
                .MaximumLength(100).WithMessage("Owner name filter cannot exceed 100 characters.")
                .When(x => x.Criteria.OwnerName is not null);
        }
    }
}
