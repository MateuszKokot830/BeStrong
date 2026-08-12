using Application.Dto.WorkoutPlan;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutTemplateExerciseCreateDtoValidatorTests
    {
        private readonly WorkoutTemplateExerciseCreateDtoValidator _validator = new();

        private static WorkoutTemplateExerciseCreateDto Valid() =>
            new(Order: 0, ExerciseId: 1, Sets: 3, MinReps: 8, MaxReps: 10);

        [Fact]
        public void Validate_WithValidExercise_HasNoErrors()
        {
            var result = _validator.TestValidate(Valid());

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenOrderIsNegative_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Order = -1 });

            result.ShouldHaveValidationErrorFor(x => x.Order);
        }

        [Fact]
        public void Validate_WhenExerciseIdIsZero_HasError()
        {
            var result = _validator.TestValidate(Valid() with { ExerciseId = 0 });

            result.ShouldHaveValidationErrorFor(x => x.ExerciseId);
        }

        [Fact]
        public void Validate_WhenSetsIsZero_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Sets = 0 });

            result.ShouldHaveValidationErrorFor(x => x.Sets);
        }

        [Fact]
        public void Validate_WhenMinRepsIsZero_HasError()
        {
            var result = _validator.TestValidate(Valid() with { MinReps = 0 });

            result.ShouldHaveValidationErrorFor(x => x.MinReps);
        }

        [Fact]
        public void Validate_WhenMaxRepsIsLessThanMinReps_HasError()
        {
            var result = _validator.TestValidate(Valid() with { MinReps = 10, MaxReps = 8 });

            result.ShouldHaveValidationErrorFor(x => x.MaxReps);
        }

        [Fact]
        public void Validate_WhenMaxRepsEqualsMinReps_HasNoError()
        {
            var result = _validator.TestValidate(Valid() with { MinReps = 10, MaxReps = 10 });

            result.ShouldNotHaveValidationErrorFor(x => x.MaxReps);
        }
    }
}
