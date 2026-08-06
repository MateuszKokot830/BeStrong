using Application.Commands.Workouts.UpdateWorkout;
using Application.Dto.Workout;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdateWorkoutCommandValidatorTests
    {
        private readonly UpdateWorkoutCommandValidator _validator = new();

        private static WorkoutExerciseDto ValidExercise() =>
            new(0, null, 1, 0, null, null, [new WorkoutSetDto(1, 10, 50, null, null)]);

        [Fact]
        public void Validate_WithValidWorkout_HasNoErrors()
        {
            var dto = new CreateWorkoutDto("Push Day", [ValidExercise()]);

            var result = _validator.TestValidate(new UpdateWorkoutCommand(1, dto));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenWorkoutIdIsNotPositive_HasError()
        {
            var dto = new CreateWorkoutDto("Push Day", [ValidExercise()]);

            var result = _validator.TestValidate(new UpdateWorkoutCommand(0, dto));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutId);
        }

        [Fact]
        public void Validate_WhenExercisesIsEmpty_HasError()
        {
            var dto = new CreateWorkoutDto("Push Day", []);

            var result = _validator.TestValidate(new UpdateWorkoutCommand(1, dto));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutDto.Exercises);
        }
    }
}
