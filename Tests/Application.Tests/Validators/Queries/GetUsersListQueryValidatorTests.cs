using Application.Helpers.Criteria;
using Application.Queries.Users.GetUsersList;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetUsersListQueryValidatorTests
    {
        private readonly GetUsersListQueryValidator _validator = new();

        private static UserSearchCriteria Valid() => new() { PageNumber = 1, PageSize = 10, Username = "ali" };

        [Fact]
        public void Validate_WithValidCriteria_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetUsersListQuery(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPageNumberIsNotPositive_HasError()
        {
            var criteria = Valid();
            criteria.PageNumber = 0;

            var result = _validator.TestValidate(new GetUsersListQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.PageNumber");
        }

        [Fact]
        public void Validate_WhenPageSizeIsNotPositive_HasError()
        {
            var criteria = Valid();
            criteria.PageSize = 0;

            var result = _validator.TestValidate(new GetUsersListQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.PageSize");
        }

        [Fact]
        public void Validate_WhenUsernameFilterExceedsMaxLength_HasError()
        {
            var criteria = Valid();
            criteria.Username = new string('a', 51);

            var result = _validator.TestValidate(new GetUsersListQuery(criteria));

            result.ShouldHaveValidationErrorFor("Criteria.Username");
        }

        [Fact]
        public void Validate_WhenUsernameFilterIsNull_HasNoError()
        {
            var criteria = Valid();
            criteria.Username = null;

            var result = _validator.TestValidate(new GetUsersListQuery(criteria));

            result.ShouldNotHaveValidationErrorFor("Criteria.Username");
        }
    }
}
