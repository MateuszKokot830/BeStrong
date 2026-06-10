using Application.Queries.Workouts.GetWorkoutStatistics;
using FluentValidation;

namespace Application.Validators.Queries
{
    public sealed class GetWorkoutStatisticsQueryValidator : AbstractValidator<GetWorkoutStatisticsQuery>
    {
        public GetWorkoutStatisticsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be a valid positive integer.");
        }
    }
}
