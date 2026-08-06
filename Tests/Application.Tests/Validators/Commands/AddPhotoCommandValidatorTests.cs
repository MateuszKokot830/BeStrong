using Application.Commands.Users.AddPhoto;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class AddPhotoCommandValidatorTests
    {
        private readonly AddPhotoCommandValidator _validator = new();

        private static AddPhotoCommand Command(int userId = 1, string fileName = "photo.jpg", long length = 100, Stream? content = null) =>
            new(content ?? Stream.Null, fileName, length, userId);

        [Fact]
        public void Validate_WithValidPhoto_HasNoErrors()
        {
            var result = _validator.TestValidate(Command());

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUserIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(Command(userId: 0));

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Validate_WhenContentIsNull_HasError()
        {
            var command = new AddPhotoCommand(null!, "photo.jpg", 100, 1);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validate_WhenLengthIsZero_HasError()
        {
            var result = _validator.TestValidate(Command(length: 0));

            result.ShouldHaveValidationErrorFor(x => x.Length);
        }

        [Fact]
        public void Validate_WhenLengthExceedsTenMegabytes_HasError()
        {
            var result = _validator.TestValidate(Command(length: 10 * 1024 * 1024 + 1));

            result.ShouldHaveValidationErrorFor(x => x.Length);
        }

        [Theory]
        [InlineData("photo.jpg")]
        [InlineData("photo.jpeg")]
        [InlineData("photo.png")]
        [InlineData("photo.gif")]
        [InlineData("photo.webp")]
        [InlineData("photo.JPG")]
        public void Validate_WithAllowedExtension_HasNoError(string fileName)
        {
            var result = _validator.TestValidate(Command(fileName: fileName));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("photo.exe")]
        [InlineData("photo.txt")]
        [InlineData("photo")]
        public void Validate_WithDisallowedExtension_HasError(string fileName)
        {
            var result = _validator.TestValidate(Command(fileName: fileName));

            Assert.False(result.IsValid);
        }
    }
}
