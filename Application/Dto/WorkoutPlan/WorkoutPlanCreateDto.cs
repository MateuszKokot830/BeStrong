using Application.Dto.Workout;

namespace Application.Dto.WorkoutPlan
{
    public record WorkoutPlanCreateDto(
        string? Name,
        string? Description,
        IReadOnlyCollection<WorkoutDto> Workouts
    );
}
