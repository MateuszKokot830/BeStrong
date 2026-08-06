using Application.Queries.Workouts.GetOneRepMax;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetOneRepMaxQueryValidatorTests
    {
        private readonly GetOneRepMaxQueryValidator _validator = new();

        [Fact]
        public void Validate_WithPositiveWeightAndReps_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetOneRepMaxQuery(100, 5));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenWeightIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new GetOneRepMaxQuery(0, 5));

            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void Validate_WhenRepsIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new GetOneRepMaxQuery(100, 0));

            result.ShouldHaveValidationErrorFor(x => x.Reps);
        }
    }
}
