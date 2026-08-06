using Application.Commands.Workouts.CreateExercise;
using Application.Dto.Exercise;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class CreateExerciseCommandValidatorTests
    {
        private readonly CreateExerciseCommandValidator _validator = new();

        private static CreateExerciseDto Valid() => new("Bench Press", "Chest exercise", MuscleSubgroup.Chest, "img.png");

        [Fact]
        public void Validate_WithValidExercise_HasNoErrors()
        {
            var result = _validator.TestValidate(new CreateExerciseCommand(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new CreateExerciseCommand(Valid() with { Name = "" }));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseDto.Name);
        }

        [Fact]
        public void Validate_WhenNameExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new CreateExerciseCommand(Valid() with { Name = new string('a', 101) }));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseDto.Name);
        }

        [Fact]
        public void Validate_WhenDescriptionIsNull_HasNoError()
        {
            var result = _validator.TestValidate(new CreateExerciseCommand(Valid() with { Description = null }));

            result.ShouldNotHaveValidationErrorFor(x => x.ExerciseDto.Description);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new CreateExerciseCommand(Valid() with { Description = new string('a', 501) }));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseDto.Description);
        }

        [Fact]
        public void Validate_WhenImageUrlExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new CreateExerciseCommand(Valid() with { ImageUrl = new string('a', 501) }));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseDto.ImageUrl);
        }
    }
}
