using Application.Commands.Register;
using Application.Dto.Auth;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class RegisterCommandValidatorTests
    {
        private readonly RegisterCommandValidator _validator = new();

        private static RegisterCommand Command(string username, string password) =>
            new(new UserRegisterRequestDto(username, password));

        [Fact]
        public void Validate_WithValidCredentials_HasNoErrors()
        {
            var result = _validator.TestValidate(Command("newuser", "Password1"));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUsernameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(Command("", "Password1"));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.UserName");
        }

        [Fact]
        public void Validate_WhenUsernameTooShort_HasError()
        {
            var result = _validator.TestValidate(Command("ab", "Password1"));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.UserName");
        }

        [Fact]
        public void Validate_WhenUsernameTooLong_HasError()
        {
            var result = _validator.TestValidate(Command(new string('a', 51), "Password1"));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.UserName");
        }

        [Fact]
        public void Validate_WhenUsernameHasInvalidCharacters_HasError()
        {
            var result = _validator.TestValidate(Command("bad name!", "Password1"));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.UserName");
        }

        [Fact]
        public void Validate_WhenPasswordIsEmpty_HasError()
        {
            var result = _validator.TestValidate(Command("newuser", ""));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.Password");
        }

        [Fact]
        public void Validate_WhenPasswordTooShort_HasError()
        {
            var result = _validator.TestValidate(Command("newuser", "Pass1"));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.Password");
        }

        [Fact]
        public void Validate_WhenPasswordHasNoDigit_HasError()
        {
            var result = _validator.TestValidate(Command("newuser", "PasswordOnly"));

            result.ShouldHaveValidationErrorFor("UserRegisterRequestDto.Password");
        }
    }
}
