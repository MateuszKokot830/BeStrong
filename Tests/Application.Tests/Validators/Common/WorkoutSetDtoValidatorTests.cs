using Application.Dto.Workout;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutSetDtoValidatorTests
    {
        private readonly WorkoutSetDtoValidator _validator = new();

        private static WorkoutSetDto Valid() => new(SetNumber: 1, Reps: 10, Weight: 50, TotalWeight: null, EstimatedOneRepMax: null);

        [Fact]
        public void Validate_WithValidSet_HasNoErrors()
        {
            var result = _validator.TestValidate(Valid());

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenSetNumberIsZero_HasError()
        {
            var result = _validator.TestValidate(Valid() with { SetNumber = 0 });

            result.ShouldHaveValidationErrorFor(x => x.SetNumber);
        }

        [Fact]
        public void Validate_WhenRepsIsZero_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Reps = 0 });

            result.ShouldHaveValidationErrorFor(x => x.Reps);
        }

        [Fact]
        public void Validate_WhenRepsExceedsMax_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Reps = 1001 });

            result.ShouldHaveValidationErrorFor(x => x.Reps);
        }

        [Fact]
        public void Validate_WhenWeightIsNull_HasNoError()
        {
            var result = _validator.TestValidate(Valid() with { Weight = null });

            result.ShouldNotHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void Validate_WhenWeightIsZero_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Weight = 0 });

            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void Validate_WhenWeightExceedsMax_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Weight = 1001 });

            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }
    }
}
