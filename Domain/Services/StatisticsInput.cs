namespace Domain.Services
{
    public record WorkoutExerciseEntry(int Sets, int ExerciseId);
    public record ExerciseEntry(int Id, string? Name);
}
