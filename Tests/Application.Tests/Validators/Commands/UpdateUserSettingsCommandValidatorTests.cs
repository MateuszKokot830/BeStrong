using Application.Commands.Users.UpdateUserSettings;
using Application.Dto.User;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdateUserSettingsCommandValidatorTests
    {
        private readonly UpdateUserSettingsCommandValidator _validator = new();

        private static UserSettingsDto Valid() => new(
            ProfileVisibility.Public, ProfileVisibility.Public, ProfileVisibility.Public, ProfileVisibility.Public,
            AutoPublishWorkouts: true, AutoPublishWorkoutPlanChanges: true);

        [Fact]
        public void Validate_WithValidSettings_HasNoErrors()
        {
            var result = _validator.TestValidate(new UpdateUserSettingsCommand(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPhotosVisibilityIsNotADefinedEnumValue_HasError()
        {
            var dto = Valid() with { PhotosVisibility = (ProfileVisibility)99 };

            var result = _validator.TestValidate(new UpdateUserSettingsCommand(dto));

            result.ShouldHaveValidationErrorFor("SettingsDto.PhotosVisibility");
        }
    }
}
