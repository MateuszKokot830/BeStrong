using Domain.ValueObjects;

namespace Domain.Services
{
    public record WorkoutExerciseEntry(int Sets, int ExerciseId);
    public record ExerciseEntry(int Id, string? Name);

    public static class StatisticsCalculator
    {
        public static Statistics Calculate(
            int workoutCount,
            IReadOnlyList<WorkoutExerciseEntry> workoutExercises,
            DateTime workoutStartDate,
            IReadOnlyList<ExerciseEntry> exercises)
        {
            var totalSets = workoutExercises.Sum(we => we.Sets);

            var totalWeeks = (DateTime.UtcNow - workoutStartDate).TotalDays / 7.0;
            var avgWorkoutsPerWeek = totalWeeks > 0
                ? Math.Round((decimal)(workoutCount / totalWeeks), 2)
                : 0m;
            var avgExercisesPerWorkout = workoutCount > 0
                ? Math.Round((decimal)workoutExercises.Count / workoutCount, 2)
                : 0m;
            var avgSetsPerWorkout = workoutCount > 0
                ? Math.Round((decimal)totalSets / workoutCount, 2)
                : 0m;

            var favouriteExerciseId = workoutExercises
                .GroupBy(we => we.ExerciseId)
                .OrderByDescending(g => g.Count())
                .Select(g => (int?)g.Key)
                .FirstOrDefault();

            var favouriteExercise = exercises
                .FirstOrDefault(e => e.Id == favouriteExerciseId)
                ?.Name;

            return new Statistics(
                workoutCount,
                workoutExercises.Count,
                totalSets,
                avgWorkoutsPerWeek,
                avgExercisesPerWorkout,
                avgSetsPerWorkout,
                favouriteExercise);
        }
    }
}
