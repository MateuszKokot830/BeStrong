using Domain.Aggregates;
using Domain.Entities;

namespace Domain.Factories
{
    public static class WorkoutFactory
    {
        public static Workout Create(int userId, string? name, IReadOnlyList<WorkoutExercise> exercises) =>
            new()
            {
                UserId = userId,
                Name = name,
                Date = DateTime.UtcNow,
                WorkoutExercises = exercises.ToList()
            };
    }
}
