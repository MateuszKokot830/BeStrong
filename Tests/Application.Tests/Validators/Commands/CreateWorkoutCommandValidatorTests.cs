using Application.Commands.Workouts.CreateWorkout;
using Application.Dto.Workout;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class CreateWorkoutCommandValidatorTests
    {
        private readonly CreateWorkoutCommandValidator _validator = new();

        private static WorkoutExerciseDto ValidExercise() =>
            new(0, null, 1, 0, null, null, [new WorkoutSetDto(1, 10, 50, null, null)]);

        [Fact]
        public void Validate_WithValidWorkout_HasNoErrors()
        {
            var dto = new CreateWorkoutDto("Push Day", [ValidExercise()]);

            var result = _validator.TestValidate(new CreateWorkoutCommand(dto));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_HasError()
        {
            var dto = new CreateWorkoutDto("", [ValidExercise()]);

            var result = _validator.TestValidate(new CreateWorkoutCommand(dto));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutDto.Name);
        }

        [Fact]
        public void Validate_WhenExercisesIsEmpty_HasError()
        {
            var dto = new CreateWorkoutDto("Push Day", []);

            var result = _validator.TestValidate(new CreateWorkoutCommand(dto));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutDto.Exercises);
        }

        [Fact]
        public void Validate_WhenAnExerciseIsInvalid_PropagatesTheChildError()
        {
            var invalidExercise = ValidExercise() with { ExerciseId = 0 };
            var dto = new CreateWorkoutDto("Push Day", [invalidExercise]);

            var result = _validator.TestValidate(new CreateWorkoutCommand(dto));

            result.ShouldHaveValidationErrorFor("WorkoutDto.Exercises[0].ExerciseId");
        }
    }
}
