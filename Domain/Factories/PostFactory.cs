using Domain.Aggregates;

namespace Domain.Factories
{
    public static class PostFactory
    {
        public static Post Create(int userId, string? description, int? workoutId, int? workoutPlanId) =>
            new()
            {
                UserId = userId,
                Description = description,
                CreatedDate = DateTime.UtcNow,
                WorkoutId = workoutId,
                WorkoutPlanId = workoutPlanId
            };
    }
}
