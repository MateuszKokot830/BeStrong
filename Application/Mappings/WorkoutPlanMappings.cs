using Application.Dto.WorkoutPlan;
using Domain.Aggregates;
using Domain.Common;

namespace Application.Mappings
{
    public static class WorkoutPlanMappings
    {
        public static WorkoutPlanDto ToDto(this WorkoutPlan plan) => new(
            plan.Id,
            plan.CreatedById,
            plan.UsedBy?.Select(u => u.ToDto()).ToList() ?? [],
            plan.Name,
            plan.Description,
            plan.Category,
            plan.Category.ToDisplayName(),
            plan.IsPublic,
            plan.WorkoutTemplates?.Select(t => t.ToDto()).ToList() ?? []
        );

        public static WorkoutPlan ToEntity(this WorkoutPlanCreateDto dto, int createdById) => new()
        {
            CreatedById = createdById,
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            IsPublic = dto.IsPublic,
            WorkoutTemplates = dto.WorkoutTemplates?.Select(t => t.ToEntity()).ToList() ?? []
        };
    }
}
