using Application.Queries.Workouts.GetWorkoutStatistics;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetWorkoutStatisticsQueryValidatorTests
    {
        private readonly GetWorkoutStatisticsQueryValidator _validator = new();

        [Fact]
        public void Validate_WithPositiveUserId_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetWorkoutStatisticsQuery(1));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUserIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new GetWorkoutStatisticsQuery(0));

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}
