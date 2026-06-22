namespace Application.Dto.Workout
{
    public record WorkoutSetDto(
        int SetNumber,
        int Reps,
        decimal? Weight,
        decimal? TotalWeight,
        int? EstimatedOneRepMax
    );
}
