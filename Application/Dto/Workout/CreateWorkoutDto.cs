namespace Application.Dto.Workout
{
    public record CreateWorkoutDto(
        string? Name,
        IReadOnlyList<WorkoutExerciseDto> Exercises
    );
}
