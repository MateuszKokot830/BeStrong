using Application.Dto.WorkoutPlan;
using Domain.Aggregates;

namespace Application.Mappings
{
    public static class WorkoutPlanMappings
    {
        public static WorkoutPlanDto ToDto(this WorkoutPlan plan) => new(
            plan.CreatedById,
            plan.UsedBy?.Select(u => u.ToDto()).ToList() ?? [],
            plan.Name,
            plan.Description,
            plan.Workouts?.Select(w => w.ToDto()).ToList() ?? []
        );

        public static WorkoutPlan ToEntity(this WorkoutPlanCreateDto dto, int createdById) => new()
        {
            CreatedById = createdById,
            Name = dto.Name,
            Description = dto.Description,
            Workouts = dto.Workouts?.Select(w => w.ToEntity()).ToList() ?? []
        };
    }
}
