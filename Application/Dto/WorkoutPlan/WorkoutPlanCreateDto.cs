using Domain.Common;

namespace Application.Dto.WorkoutPlan
{
    public record WorkoutPlanCreateDto(
        string? Name,
        string? Description,
        WorkoutPlanCategory Category,
        bool IsPublic,
        IReadOnlyCollection<WorkoutTemplateDto> WorkoutTemplates
    );
}
