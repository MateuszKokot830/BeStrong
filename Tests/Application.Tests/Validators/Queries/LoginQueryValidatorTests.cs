using Application.Dto.Auth;
using Application.Queries.Login;
using Application.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Queries
{
    public class LoginQueryValidatorTests
    {
        private readonly LoginQueryValidator _validator = new();

        [Fact]
        public void Validate_WithValidCredentials_HasNoErrors()
        {
            var result = _validator.TestValidate(new LoginQuery(new UserLoginRequestDto("existinguser", "Password1")));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUsernameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new LoginQuery(new UserLoginRequestDto("", "Password1")));

            result.ShouldHaveValidationErrorFor("UserLoginRequestDto.UserName");
        }

        [Fact]
        public void Validate_WhenPasswordIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new LoginQuery(new UserLoginRequestDto("existinguser", "")));

            result.ShouldHaveValidationErrorFor("UserLoginRequestDto.Password");
        }
    }
}
