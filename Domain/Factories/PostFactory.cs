using Domain.Aggregates;
using Domain.Common;

namespace Domain.Factories
{
    public static class PostFactory
    {
        public static Post Create(int userId, PostType type, string? description, int? workoutId, int? workoutPlanId) =>
            new()
            {
                UserId = userId,
                Type = type,
                Description = description,
                CreatedDate = DateTime.UtcNow,
                WorkoutId = workoutId,
                WorkoutPlanId = workoutPlanId
            };
    }
}
