using Application.Queries.Workouts.GetWorkouts;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetWorkoutsQueryValidator : AbstractValidator<GetWorkoutsQuery>
    {
        public GetWorkoutsQueryValidator()
        {
            RuleFor(x => x.Criteria.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Criteria.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50.");

            RuleFor(x => x.Criteria.Name)
                .MaximumLength(100).WithMessage("Name filter cannot exceed 100 characters.")
                .When(x => x.Criteria.Name is not null);

            RuleFor(x => x.Criteria.DateTo)
                .GreaterThanOrEqualTo(x => x.Criteria.DateFrom).WithMessage("Date to cannot be earlier than date from.")
                .When(x => x.Criteria.DateFrom is not null && x.Criteria.DateTo is not null);
        }
    }
}
