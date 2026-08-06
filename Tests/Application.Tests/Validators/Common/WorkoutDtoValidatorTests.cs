using Application.Dto.Workout;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutDtoValidatorTests
    {
        private readonly WorkoutDtoValidator _validator = new();

        private static WorkoutExerciseDto ValidExercise() =>
            new(0, null, 1, 1, null, null, [new WorkoutSetDto(1, 10, 50, null, null)]);

        [Fact]
        public void Validate_WithValidWorkout_HasNoErrors()
        {
            var dto = new WorkoutDto(0, 1, DateTime.UtcNow, "Push Day", [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_HasError()
        {
            var dto = new WorkoutDto(0, 1, DateTime.UtcNow, "", [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenExercisesIsEmpty_HasError()
        {
            var dto = new WorkoutDto(0, 1, DateTime.UtcNow, "Push Day", []);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.WorkoutExercises);
        }
    }
}
