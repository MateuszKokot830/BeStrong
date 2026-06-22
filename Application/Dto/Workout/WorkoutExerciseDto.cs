namespace Application.Dto.Workout
{
    public record WorkoutExerciseDto(
        int Order,
        string? Notes,
        int ExerciseId,
        int WorkoutId,
        decimal? MaxTotalWeight,
        int? BestEstimatedOneRepMax,
        IReadOnlyList<WorkoutSetDto> Sets
    );
}
