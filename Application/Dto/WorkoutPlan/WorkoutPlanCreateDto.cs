using Application.Dto.Workout;

namespace Application.Dto.WorkoutPlan
{
    public record WorkoutPlanCreateDto(
        int CreatedById,
        string? Name,
        string? Description,
        IReadOnlyCollection<WorkoutDto> Workouts
    );
}