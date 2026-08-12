namespace Application.Dto.WorkoutPlan
{
    public record WorkoutTemplateExerciseCreateDto(
        int Order,
        int ExerciseId,
        int Sets,
        int MinReps,
        int MaxReps
    );
}
