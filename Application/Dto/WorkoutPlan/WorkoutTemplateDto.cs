namespace Application.Dto.WorkoutPlan
{
    public record WorkoutTemplateDto(
        int Order,
        string? Name,
        IReadOnlyList<WorkoutTemplateExerciseDto> Exercises
    );
}
