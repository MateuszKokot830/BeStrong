using Application.Dto.User;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class MeasurementsDtoValidatorTests
    {
        private readonly MeasurementsDtoValidator _validator = new();

        private static MeasurementsDto AllNull() => new(null, null, null, null, null, null, null, null);

        [Fact]
        public void Validate_WithAllFieldsNull_HasNoErrors()
        {
            var result = _validator.TestValidate(AllNull());

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(49)]
        [InlineData(301)]
        public void Validate_WhenHeightOutOfRange_HasError(int height)
        {
            var result = _validator.TestValidate(AllNull() with { Height = height });

            result.ShouldHaveValidationErrorFor(x => x.Height);
        }

        [Fact]
        public void Validate_WhenHeightInRange_HasNoError()
        {
            var result = _validator.TestValidate(AllNull() with { Height = 180 });

            result.ShouldNotHaveValidationErrorFor(x => x.Height);
        }

        [Theory]
        [InlineData(19)]
        [InlineData(701)]
        public void Validate_WhenWeightOutOfRange_HasError(decimal weight)
        {
            var result = _validator.TestValidate(AllNull() with { Weight = weight });

            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void Validate_WhenChestIsZeroOrNegative_HasError()
        {
            var result = _validator.TestValidate(AllNull() with { Chest = 0 });

            result.ShouldHaveValidationErrorFor(x => x.Chest);
        }

        [Fact]
        public void Validate_WhenChestExceedsMax_HasError()
        {
            var result = _validator.TestValidate(AllNull() with { Chest = 301 });

            result.ShouldHaveValidationErrorFor(x => x.Chest);
        }

        [Fact]
        public void Validate_WhenArmsExceedsMax_HasError()
        {
            var result = _validator.TestValidate(AllNull() with { Arms = 201 });

            result.ShouldHaveValidationErrorFor(x => x.Arms);
        }

        [Fact]
        public void Validate_WhenThightsExceedsMax_HasError()
        {
            var result = _validator.TestValidate(AllNull() with { Thights = 201 });

            result.ShouldHaveValidationErrorFor(x => x.Thights);
        }

        [Fact]
        public void Validate_WithAllFieldsInRange_HasNoErrors()
        {
            var dto = new MeasurementsDto(180, 80, 100, 110, 35, 85, 95, 55);

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
