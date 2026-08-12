using Application.Commands.WorkoutPlans.CreateWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class CreateWorkoutPlanCommandValidatorTests
    {
        private readonly CreateWorkoutPlanCommandValidator _validator = new();

        private static WorkoutTemplateCreateDto ValidTemplate() =>
            new(0, "Day A", [new WorkoutTemplateExerciseCreateDto(0, 1, Sets: 3, MinReps: 8, MaxReps: 10)]);

        private static WorkoutPlanCreateDto Valid() =>
            new("Push Pull Legs", "desc", WorkoutPlanCategory.PushPullLegs, IsPublic: true, [ValidTemplate()]);

        [Fact]
        public void Validate_WithValidPlan_HasNoErrors()
        {
            var result = _validator.TestValidate(new CreateWorkoutPlanCommand(Valid()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new CreateWorkoutPlanCommand(Valid() with { Name = "" }));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutPlanCreateDto.Name);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new CreateWorkoutPlanCommand(Valid() with { Description = new string('a', 1001) }));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutPlanCreateDto.Description);
        }

        [Fact]
        public void Validate_WhenWorkoutTemplatesIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new CreateWorkoutPlanCommand(Valid() with { WorkoutTemplates = [] }));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutPlanCreateDto.WorkoutTemplates);
        }

        [Fact]
        public void Validate_WhenATemplateIsInvalid_PropagatesTheChildError()
        {
            var invalidTemplate = ValidTemplate() with { Exercises = [] };

            var result = _validator.TestValidate(new CreateWorkoutPlanCommand(Valid() with { WorkoutTemplates = [invalidTemplate] }));

            result.ShouldHaveValidationErrorFor("WorkoutPlanCreateDto.WorkoutTemplates[0].Exercises");
        }
    }
}
