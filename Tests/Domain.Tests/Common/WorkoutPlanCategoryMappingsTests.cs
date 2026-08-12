using Domain.Common;

namespace Domain.Tests.Common
{
    public class WorkoutPlanCategoryMappingsTests
    {
        [Theory]
        [InlineData(WorkoutPlanCategory.FullBody, "Full Body")]
        [InlineData(WorkoutPlanCategory.PushPullLegs, "Push Pull Legs")]
        [InlineData(WorkoutPlanCategory.PushPull, "Push Pull")]
        [InlineData(WorkoutPlanCategory.UpperLower, "Upper Lower")]
        [InlineData(WorkoutPlanCategory.BodyPartSplit, "Body Part Split")]
        public void ToDisplayName_MapsCategoryToExpectedLabel(WorkoutPlanCategory category, string expected)
        {
            var result = category.ToDisplayName();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayName_EveryDefinedCategoryHasAMapping()
        {
            foreach (WorkoutPlanCategory category in Enum.GetValues<WorkoutPlanCategory>())
            {
                var exception = Record.Exception(() => category.ToDisplayName());

                Assert.Null(exception);
            }
        }
    }
}
