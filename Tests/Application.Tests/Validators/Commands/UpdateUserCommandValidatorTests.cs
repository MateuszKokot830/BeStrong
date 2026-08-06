using Application.Commands.Users.UpdateUser;
using Application.Dto.User;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdateUserCommandValidatorTests
    {
        private readonly UpdateUserCommandValidator _validator = new();

        private static UserUpdateDto Valid() => new(
            Id: 1,
            DateOfBirth: DateTime.UtcNow.AddYears(-25),
            DateOfWorkoutStart: DateTime.UtcNow.AddYears(-1),
            Name: "Alice",
            Surname: "Smith",
            Gender: Gender.Female,
            City: "Warsaw",
            Country: "Poland",
            Description: "hi",
            Measurements: new MeasurementsDto(180, 80, null, null, null, null, null, null),
            Photos: []);

        [Fact]
        public void Validate_WithValidUpdate_HasNoErrors()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Id = 0 }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.Id);
        }

        [Fact]
        public void Validate_WhenDateOfBirthIsInTheFuture_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { DateOfBirth = DateTime.UtcNow.AddDays(1) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.DateOfBirth);
        }

        [Fact]
        public void Validate_WhenDateOfBirthIsUnrealisticallyOld_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { DateOfBirth = DateTime.UtcNow.AddYears(-121) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.DateOfBirth);
        }

        [Fact]
        public void Validate_WhenDateOfWorkoutStartIsNull_HasNoError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { DateOfWorkoutStart = null }));

            result.ShouldNotHaveValidationErrorFor(x => x.UserUpdateDto.DateOfWorkoutStart);
        }

        [Fact]
        public void Validate_WhenDateOfWorkoutStartIsInTheFuture_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { DateOfWorkoutStart = DateTime.UtcNow.AddDays(1) }));

            result.ShouldHaveValidationErrorFor("UserUpdateDto.DateOfWorkoutStart.Value");
        }

        [Fact]
        public void Validate_WhenNameExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Name = new string('a', 51) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.Name);
        }

        [Fact]
        public void Validate_WhenSurnameExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Surname = new string('a', 51) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.Surname);
        }

        [Fact]
        public void Validate_WhenCityExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { City = new string('a', 101) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.City);
        }

        [Fact]
        public void Validate_WhenCountryExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Country = new string('a', 101) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.Country);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Description = new string('a', 1001) }));

            result.ShouldHaveValidationErrorFor(x => x.UserUpdateDto.Description);
        }

        [Fact]
        public void Validate_WhenMeasurementsIsNull_HasNoError()
        {
            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Measurements = null }));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenMeasurementsAreInvalid_PropagatesTheChildError()
        {
            var invalidMeasurements = new MeasurementsDto(Height: 10, null, null, null, null, null, null, null);

            var result = _validator.TestValidate(new UpdateUserCommand(Valid() with { Measurements = invalidMeasurements }));

            result.ShouldHaveValidationErrorFor("UserUpdateDto.Measurements.Height");
        }
    }
}
