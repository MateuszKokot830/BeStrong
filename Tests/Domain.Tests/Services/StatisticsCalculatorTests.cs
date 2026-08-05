using Domain.Services;

namespace Domain.Tests.Services
{
    public class StatisticsCalculatorTests
    {
        private static readonly IReadOnlyList<ExerciseEntry> Exercises =
        [
            new ExerciseEntry(1, "Bench Press"),
            new ExerciseEntry(2, "Squat")
        ];

        [Fact]
        public void Calculate_WithNoWorkouts_ReturnsAllZeroAverages()
        {
            var result = StatisticsCalculator.Calculate(
                workoutCount: 0,
                workoutExercises: [],
                workoutStartDate: null,
                exercises: Exercises);

            Assert.Equal(0, result.TotalWorkouts);
            Assert.Equal(0, result.TotalExercises);
            Assert.Equal(0, result.TotalSets);
            Assert.Equal(0m, result.AvgWorkoutsPerWeek);
            Assert.Equal(0m, result.AvgExercisesPerWorkout);
            Assert.Equal(0m, result.AvgSetsPerWorkout);
            Assert.Null(result.FavouriteExercise);
        }

        [Fact]
        public void Calculate_WithNullStartDate_AvgWorkoutsPerWeekIsZero()
        {
            var workoutExercises = new List<WorkoutExerciseEntry> { new(Sets: 3, ExerciseId: 1) };

            var result = StatisticsCalculator.Calculate(
                workoutCount: 5,
                workoutExercises: workoutExercises,
                workoutStartDate: null,
                exercises: Exercises);

            Assert.Equal(0m, result.AvgWorkoutsPerWeek);
        }

        [Fact]
        public void Calculate_WithStartDateOneWeekAgo_AvgWorkoutsPerWeekMatchesWorkoutCount()
        {
            var result = StatisticsCalculator.Calculate(
                workoutCount: 3,
                workoutExercises: [],
                workoutStartDate: DateTime.UtcNow.AddDays(-7),
                exercises: Exercises);

            Assert.Equal(3m, result.AvgWorkoutsPerWeek);
        }

        [Fact]
        public void Calculate_TotalSetsIsSumOfAllExerciseSets()
        {
            var workoutExercises = new List<WorkoutExerciseEntry>
            {
                new(Sets: 3, ExerciseId: 1),
                new(Sets: 4, ExerciseId: 2),
                new(Sets: 5, ExerciseId: 1)
            };

            var result = StatisticsCalculator.Calculate(
                workoutCount: 2,
                workoutExercises: workoutExercises,
                workoutStartDate: null,
                exercises: Exercises);

            Assert.Equal(12, result.TotalSets);
            Assert.Equal(6m, result.AvgSetsPerWorkout);
            Assert.Equal(1.5m, result.AvgExercisesPerWorkout);
        }

        [Fact]
        public void Calculate_FavouriteExercise_IsTheOneAppearingMostOften()
        {
            var workoutExercises = new List<WorkoutExerciseEntry>
            {
                new(Sets: 3, ExerciseId: 1),
                new(Sets: 3, ExerciseId: 2),
                new(Sets: 3, ExerciseId: 1)
            };

            var result = StatisticsCalculator.Calculate(
                workoutCount: 2,
                workoutExercises: workoutExercises,
                workoutStartDate: null,
                exercises: Exercises);

            Assert.Equal("Bench Press", result.FavouriteExercise);
        }

        [Fact]
        public void Calculate_FavouriteExerciseId_NotFoundInExerciseList_ReturnsNullName()
        {
            var workoutExercises = new List<WorkoutExerciseEntry> { new(Sets: 3, ExerciseId: 999) };

            var result = StatisticsCalculator.Calculate(
                workoutCount: 1,
                workoutExercises: workoutExercises,
                workoutStartDate: null,
                exercises: Exercises);

            Assert.Null(result.FavouriteExercise);
        }

        [Fact]
        public void Calculate_WithNoWorkoutExercises_FavouriteExerciseIsNull()
        {
            var result = StatisticsCalculator.Calculate(
                workoutCount: 2,
                workoutExercises: [],
                workoutStartDate: null,
                exercises: Exercises);

            Assert.Null(result.FavouriteExercise);
        }
    }
}
