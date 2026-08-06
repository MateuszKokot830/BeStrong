using Application.Dto.WorkoutPlan;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutTemplateDtoValidatorTests
    {
        private readonly WorkoutTemplateDtoValidator _validator = new();

        private static WorkoutTemplateExerciseDto ValidExercise() => new(Order: 0, ExerciseId: 1);

        [Fact]
        public void Validate_WithValidTemplate_HasNoErrors()
        {
            var dto = new WorkoutTemplateDto(Order: 0, Name: "Day A", Exercises: [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenNameIsNull_HasNoError()
        {
            var dto = new WorkoutTemplateDto(Order: 0, Name: null, Exercises: [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenNameExceedsMaxLength_HasError()
        {
            var dto = new WorkoutTemplateDto(Order: 0, Name: new string('a', 101), Exercises: [ValidExercise()]);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenExercisesIsEmpty_HasError()
        {
            var dto = new WorkoutTemplateDto(Order: 0, Name: "Day A", Exercises: []);

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Exercises);
        }
    }
}
