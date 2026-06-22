using Application.Dto.User;
using Domain.Common;

namespace Application.Dto.WorkoutPlan
{
    public record WorkoutPlanDto(
        int Id,
        int CreatedById,
        IReadOnlyCollection<UserDto> UsedBy,
        string? Name,
        string? Description,
        WorkoutPlanCategory Category,
        bool IsPublic,
        IReadOnlyCollection<WorkoutTemplateDto> WorkoutTemplates
    );
}
