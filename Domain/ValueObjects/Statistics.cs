namespace Domain.ValueObjects
{
    public record Statistics(
        int TotalWorkouts,
        int TotalExercises,
        int TotalSets,
        decimal AvgWorkoutsPerWeek,
        decimal AvgExercisesPerWorkout,
        decimal AvgSetsPerWorkout,
        string? FavouriteExercise
    );
}
