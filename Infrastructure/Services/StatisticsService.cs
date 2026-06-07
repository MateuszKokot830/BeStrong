using Application.Dto.Exercise;
using Application.Dto.Statistics;
using Application.Dto.Workout;
using Application.Interfaces.Services;

namespace Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        public StatisticsDto Calculate(
            IReadOnlyList<WorkoutDto> workouts,
            DateTime workoutStartDate,
            IReadOnlyList<ExerciseDto> exercises)
        {
            var totalWorkouts = workouts.Count;
            var workoutExercises = workouts.SelectMany(x => x.WorkoutExercises).ToList();
            var totalExercises = workoutExercises.Count;
            var totalSets = workoutExercises.Sum(x => x.Sets);

            var totalWeeks = (DateTime.Now - workoutStartDate).TotalDays / 7.0;
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
                .GroupBy(x => x.ExerciseId)
                .OrderByDescending(g => g.Count())
                .Select(g => (int?)g.Key)
                .FirstOrDefault();

            var favouriteExercise = exercises
                .FirstOrDefault(x => x.Id == favouriteExerciseId)
                ?.Name;

            return new StatisticsDto(
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
