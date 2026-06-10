using Application.Queries.Workouts.GetUserWorkouts;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetUserWorkoutsQueryValidator : AbstractValidator<GetUserWorkoutsQuery>
    {
        public GetUserWorkoutsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");
        }
    }
}
