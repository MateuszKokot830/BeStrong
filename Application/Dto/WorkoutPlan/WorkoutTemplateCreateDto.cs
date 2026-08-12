namespace Application.Dto.WorkoutPlan
{
    public record WorkoutTemplateCreateDto(
        int Order,
        string? Name,
        IReadOnlyList<WorkoutTemplateExerciseCreateDto> Exercises
    );
}
