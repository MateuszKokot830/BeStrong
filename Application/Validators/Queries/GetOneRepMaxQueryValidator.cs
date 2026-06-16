using Application.Queries.Workouts.GetOneRepMax;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetOneRepMaxQueryValidator : AbstractValidator<GetOneRepMaxQuery>
    {
        public GetOneRepMaxQueryValidator()
        {
            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0.");

            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Reps must be greater than 0.");
        }
    }
}
