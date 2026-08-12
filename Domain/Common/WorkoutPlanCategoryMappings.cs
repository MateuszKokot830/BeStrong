namespace Domain.Common
{
    public static class WorkoutPlanCategoryMappings
    {
        private static readonly Dictionary<WorkoutPlanCategory, string> _displayNames = new()
        {
            [WorkoutPlanCategory.FullBody] = "Full Body",
            [WorkoutPlanCategory.PushPullLegs] = "Push Pull Legs",
            [WorkoutPlanCategory.PushPull] = "Push Pull",
            [WorkoutPlanCategory.UpperLower] = "Upper Lower",
            [WorkoutPlanCategory.BodyPartSplit] = "Body Part Split",
        };

        public static string ToDisplayName(this WorkoutPlanCategory category) => _displayNames[category];
    }
}
