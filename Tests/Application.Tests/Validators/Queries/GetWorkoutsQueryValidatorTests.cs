using Application.Helpers.Criteria;
using Application.Queries.Workouts.GetWorkouts;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetWorkoutsQueryValidatorTests
    {
        private readonly GetWorkoutsQueryValidator _validator = new();

        private static WorkoutSearchCriteria Valid() => new() { PageNumber = 1, PageSize = 10, Name = "Push Day" };

        [Fact]
        public void Validate_WithValidCriteria_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetWorkoutsQuery(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPageNumberIsNotPositive_HasError()
        {
            var criteria = Valid();
            criteria.PageNumber = 0;

            var result = _validator.TestValidate(new GetWorkoutsQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.PageNumber");
        }

        [Fact]
        public void Validate_WhenPageSizeIsNotPositive_HasError()
        {
            var criteria = Valid();
            criteria.PageSize = 0;

            var result = _validator.TestValidate(new GetWorkoutsQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.PageSize");
        }

        [Fact]
        public void Validate_WhenNameFilterExceedsMaxLength_HasError()
        {
            var criteria = Valid();
            criteria.Name = new string('a', 101);

            var result = _validator.TestValidate(new GetWorkoutsQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.Name");
        }

        [Fact]
        public void Validate_WhenDateToIsBeforeDateFrom_HasError()
        {
            var criteria = Valid();
            criteria.DateFrom = new DateTime(2026, 6, 1);
            criteria.DateTo = new DateTime(2026, 5, 1);

            var result = _validator.TestValidate(new GetWorkoutsQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.DateTo");
        }

        [Fact]
        public void Validate_WhenDateToIsOnOrAfterDateFrom_HasNoError()
        {
            var criteria = Valid();
            criteria.DateFrom = new DateTime(2026, 6, 1);
            criteria.DateTo = new DateTime(2026, 6, 30);

            var result = _validator.TestValidate(new GetWorkoutsQuery(criteria));

            result.ShouldNotHaveValidationErrorFor("Criteria.DateTo");
        }
    }
}
