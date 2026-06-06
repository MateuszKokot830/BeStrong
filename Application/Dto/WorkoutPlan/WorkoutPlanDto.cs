using Application.Dto.User;
using Application.Dto.Workout;

namespace Application.Dto.WorkoutPlan
{
    public record WorkoutPlanDto(
        int CreatedById,
        IReadOnlyCollection<UserDto> UsedBy,
        string? Name,
        string? Description,
        IReadOnlyCollection<WorkoutDto> Workouts
    );
}