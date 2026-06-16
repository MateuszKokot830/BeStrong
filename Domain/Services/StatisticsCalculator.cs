using Domain.Aggregates;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services
{
    public static class StatisticsCalculator
    {
        public static Statistics Calculate(
            IReadOnlyList<Workout> workouts,
            DateTime workoutStartDate,
            IReadOnlyList<Exercise> exercises)
        {
            var totalWorkouts = workouts.Count;
            var workoutExercises = workouts.SelectMany(w => w.WorkoutExercises).ToList();
            var totalExercises = workoutExercises.Count;
            var totalSets = workoutExercises.Sum(we => we.Sets);

            var totalWeeks = (DateTime.UtcNow - workoutStartDate).TotalDays / 7.0;
            var avgWorkoutsPerWeek = totalWeeks > 0
                ? Math.Round((decimal)(totalWorkouts / totalWeeks), 2)
                : 0m;
            var avgExercisesPerWorkout = totalWorkouts > 0
                ? Math.Round((decimal)totalExercises / totalWorkouts, 2)
                : 0m;
            var avgSetsPerWorkout = totalWorkouts > 0
                ? Math.Round((decimal)totalSets / totalWorkouts, 2)
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
                totalWorkouts,
                totalExercises,
                totalSets,
                avgWorkoutsPerWeek,
                avgExercisesPerWorkout,
                avgSetsPerWorkout,
                favouriteExercise);
        }
    }
}
