using Application.Queries.Users.GetUserByUsername;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class GetUserByUsernameQueryValidatorTests
    {
        private readonly GetUserByUsernameQueryValidator _validator = new();

        [Fact]
        public void Validate_WithValidUsername_HasNoErrors()
        {
            var result = _validator.TestValidate(new GetUserByUsernameQuery("alice"));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUsernameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new GetUserByUsernameQuery(""));

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Validate_WhenUsernameExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new GetUserByUsernameQuery(new string('a', 51)));

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }
    }
}
