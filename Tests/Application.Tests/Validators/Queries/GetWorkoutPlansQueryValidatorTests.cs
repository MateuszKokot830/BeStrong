using Application.Helpers.Criteria;
using Application.Queries.WorkoutPlans.GetWorkoutPlans;
using Application.Validators.Queries;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetWorkoutPlansQueryValidatorTests
    {
        private readonly GetWorkoutPlansQueryValidator _validator = new();

        private static WorkoutPlanSearchCriteria Valid() => new() { PageNumber = 1, PageSize = 10, Name = "PPL" };

        [Fact]
        public void Validate_WithValidCriteria_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetWorkoutPlansQuery(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPageNumberIsNotPositive_HasError()
        {
            var criteria = Valid();
            criteria.PageNumber = 0;

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.PageNumber");
        }

        [Fact]
        public void Validate_WhenPageSizeIsNotPositive_HasError()
        {
            var criteria = Valid();
            criteria.PageSize = 0;

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.PageSize");
        }

        [Fact]
        public void Validate_WhenNameFilterExceedsMaxLength_HasError()
        {
            var criteria = Valid();
            criteria.Name = new string('a', 101);

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.Name");
        }

        [Fact]
        public void Validate_WhenNameFilterIsNull_HasNoError()
        {
            var criteria = Valid();
            criteria.Name = null;

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldNotHaveValidationErrorFor("Criteria.Name");
        }

        [Fact]
        public void Validate_WhenOwnerNameFilterExceedsMaxLength_HasError()
        {
            var criteria = Valid();
            criteria.OwnerName = new string('a', 101);

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.OwnerName");
        }

        [Fact]
        public void Validate_WhenOwnerNameFilterIsNull_HasNoError()
        {
            var criteria = Valid();
            criteria.OwnerName = null;

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldNotHaveValidationErrorFor("Criteria.OwnerName");
        }

        [Fact]
        public void Validate_WhenCategoryIsNull_HasNoError()
        {
            var criteria = Valid();
            criteria.Category = null;

            var result = _validator.TestValidate(new GetWorkoutPlansQuery(criteria));

            result.ShouldNotHaveValidationErrorFor(x => x.Criteria.Category);
        }
    }
}
