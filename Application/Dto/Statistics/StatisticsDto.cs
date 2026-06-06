namespace Application.Dto.Statistics
{
    public record StatisticsDto(
        int TotalWorkouts,
        int TotalExercises,
        int TotalSets,
        decimal AvgWorkoutsPerWeek,
        decimal AvgExercisesPerWorkout,
        decimal AvgSetsPerWorkout,
        string? FavouriteExercise
    );
}