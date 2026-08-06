using Application.Queries.Users.GetUsersByIds;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetUsersByIdsQueryValidatorTests
    {
        private readonly GetUsersByIdsQueryValidator _validator = new();

        [Fact]
        public void Validate_WithPositiveIds_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetUsersByIdsQuery([1, 2, 3]));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenIdsIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new GetUsersByIdsQuery([]));

            result.ShouldHaveValidationErrorFor(x => x.UserIds);
        }

        [Fact]
        public void Validate_WhenAnIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new GetUsersByIdsQuery([1, 0, 3]));

            result.ShouldHaveValidationErrorFor("UserIds[1]");
        }
    }
}
