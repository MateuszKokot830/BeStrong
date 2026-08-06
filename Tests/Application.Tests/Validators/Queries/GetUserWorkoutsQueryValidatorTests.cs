using Application.Queries.Workouts.GetUserWorkouts;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetUserWorkoutsQueryValidatorTests
    {
        private readonly GetUserWorkoutsQueryValidator _validator = new();

        [Fact]
        public void Validate_WithPositiveUserId_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetUserWorkoutsQuery(1));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUserIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new GetUserWorkoutsQuery(0));

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}
