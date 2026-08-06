using Application.Commands.Workouts.UpdateExercise;
using Application.Dto.Exercise;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdateExerciseCommandValidatorTests
    {
        private readonly UpdateExerciseCommandValidator _validator = new();

        private static CreateExerciseDto ValidDto() => new("Bench Press", null, MuscleSubgroup.Chest, null);

        [Fact]
        public void Validate_WithValidUpdate_HasNoErrors()
        {
            var result = _validator.TestValidate(new UpdateExerciseCommand(1, ValidDto()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenExerciseIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new UpdateExerciseCommand(0, ValidDto()));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseId);
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new UpdateExerciseCommand(1, ValidDto() with { Name = "" }));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseDto.Name);
        }
    }
}
