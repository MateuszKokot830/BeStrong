using Application.Dto.WorkoutPlan;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutTemplateCreateDtoValidatorTests
    {
        private readonly WorkoutTemplateCreateDtoValidator _validator = new();

        private static WorkoutTemplateExerciseCreateDto ValidExercise() =>
            new(Order: 0, ExerciseId: 1, Sets: 3, MinReps: 8, MaxReps: 10);

        [Fact]
        public void Validate_WithValidTemplate_HasNoErrors()
        {
            var dto = new WorkoutTemplateCreateDto(Order: 0, Name: "Day A", Exercises: [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenNameIsNull_HasNoError()
        {
            var dto = new WorkoutTemplateCreateDto(Order: 0, Name: null, Exercises: [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenNameExceedsMaxLength_HasError()
        {
            var dto = new WorkoutTemplateCreateDto(Order: 0, Name: new string('a', 101), Exercises: [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenExercisesIsEmpty_HasError()
        {
            var dto = new WorkoutTemplateCreateDto(Order: 0, Name: "Day A", Exercises: []);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Exercises);
        }
    }
}
