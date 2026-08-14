using Application.Commands.WorkoutPlans.UpdateWorkoutPlan;
using Application.Dto.WorkoutPlan;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdateWorkoutPlanCommandValidatorTests
    {
        private readonly UpdateWorkoutPlanCommandValidator _validator = new();

        private static WorkoutTemplateCreateDto ValidTemplate() =>
            new(0, "Day A", [new WorkoutTemplateExerciseCreateDto(0, 1, Sets: 3, MinReps: 8, MaxReps: 10)]);

        private static WorkoutPlanCreateDto ValidDto() =>
            new("Push Pull Legs", "desc", WorkoutPlanCategory.PushPullLegs, IsPublic: true, [ValidTemplate()]);

        [Fact]
        public void Validate_WithValidCommand_HasNoErrors()
        {
            var result = _validator.TestValidate(new UpdateWorkoutPlanCommand(1, ValidDto()));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPlanIdIsZero_HasError()
        {
            var result = _validator.TestValidate(new UpdateWorkoutPlanCommand(0, ValidDto()));

            result.ShouldHaveValidationErrorFor(x => x.PlanId);
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new UpdateWorkoutPlanCommand(1, ValidDto() with { Name = "" }));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutPlanDto.Name);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateWorkoutPlanCommand(1, ValidDto() with { Description = new string('a', 1001) }));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutPlanDto.Description);
        }

        [Fact]
        public void Validate_WhenWorkoutTemplatesIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new UpdateWorkoutPlanCommand(1, ValidDto() with { WorkoutTemplates = [] }));

            result.ShouldHaveValidationErrorFor(x => x.WorkoutPlanDto.WorkoutTemplates);
        }

        [Fact]
        public void Validate_WhenATemplateIsInvalid_PropagatesTheChildError()
        {
            var invalidTemplate = ValidTemplate() with { Exercises = [] };

            var result = _validator.TestValidate(new UpdateWorkoutPlanCommand(1, ValidDto() with { WorkoutTemplates = [invalidTemplate] }));

            result.ShouldHaveValidationErrorFor("WorkoutPlanDto.WorkoutTemplates[0].Exercises");
        }
    }
}
