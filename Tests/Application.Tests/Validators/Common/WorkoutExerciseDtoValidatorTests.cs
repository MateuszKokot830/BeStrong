using Application.Dto.Workout;
using Application.Validators.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Common
{
    public class WorkoutExerciseDtoValidatorTests
    {
        private readonly WorkoutExerciseDtoValidator _validator = new();

        private static WorkoutSetDto ValidSet() => new(1, 10, 50, null, null);

        private static WorkoutExerciseDto Valid() => new(
            Order: 0, Notes: null, ExerciseId: 1, WorkoutId: 1, MaxTotalWeight: null, BestEstimatedOneRepMax: null, Sets: [ValidSet()]);

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
        public void Validate_WhenNotesExceedMaxLength_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Notes = new string('a', 501) });

            result.ShouldHaveValidationErrorFor(x => x.Notes);
        }

        [Fact]
        public void Validate_WhenSetsIsEmpty_HasError()
        {
            var result = _validator.TestValidate(Valid() with { Sets = [] });

            result.ShouldHaveValidationErrorFor(x => x.Sets);
        }

        [Fact]
        public void Validate_WhenASetIsInvalid_PropagatesTheChildError()
        {
            var invalidSet = ValidSet() with { Reps = 0 };

            var result = _validator.TestValidate(Valid() with { Sets = [invalidSet] });

            result.ShouldHaveValidationErrorFor("Sets[0].Reps");
        }
    }
}
