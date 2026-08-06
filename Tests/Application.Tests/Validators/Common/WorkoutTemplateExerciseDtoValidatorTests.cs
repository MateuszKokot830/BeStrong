using Application.Dto.WorkoutPlan;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutTemplateExerciseDtoValidatorTests
    {
        private readonly WorkoutTemplateExerciseDtoValidator _validator = new();

        [Fact]
        public void Validate_WithValidExercise_HasNoErrors()
        {
            var result = _validator.TestValidate(new WorkoutTemplateExerciseDto(Order: 0, ExerciseId: 1));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenOrderIsNegative_HasError()
        {
            var result = _validator.TestValidate(new WorkoutTemplateExerciseDto(Order: -1, ExerciseId: 1));

            result.ShouldHaveValidationErrorFor(x => x.Order);
        }

        [Fact]
        public void Validate_WhenExerciseIdIsZero_HasError()
        {
            var result = _validator.TestValidate(new WorkoutTemplateExerciseDto(Order: 0, ExerciseId: 0));

            result.ShouldHaveValidationErrorFor(x => x.ExerciseId);
        }
    }
}
