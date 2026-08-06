using Application.Commands.Users.DeletePhoto;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class DeletePhotoCommandValidatorTests
    {
        private readonly DeletePhotoCommandValidator _validator = new();

        [Fact]
        public void Validate_WithPositiveIds_HasNoErrors()
        {
            var result = _validator.TestValidate(new DeletePhotoCommand(PhotoId: 1, UserId: 1));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPhotoIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new DeletePhotoCommand(PhotoId: 0, UserId: 1));

            result.ShouldHaveValidationErrorFor(x => x.PhotoId);
        }

        [Fact]
        public void Validate_WhenUserIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new DeletePhotoCommand(PhotoId: 1, UserId: 0));

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }
    }
}
