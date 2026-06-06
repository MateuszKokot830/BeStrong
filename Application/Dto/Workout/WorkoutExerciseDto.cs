namespace Application.Dto.Workout
{
    public record WorkoutExerciseDto(
        int Sets,
        int Reps,
        decimal? Weight,
        int ExerciseId,
        int WorkoutId
    );
}