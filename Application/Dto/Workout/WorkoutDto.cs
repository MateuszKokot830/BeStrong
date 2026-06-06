namespace Application.Dto.Workout
{
    public record WorkoutDto(
        int Id,
        int? UserId,
        DateTime Date,
        string? Name,
        IReadOnlyCollection<WorkoutExerciseDto> WorkoutExercises
    );
}