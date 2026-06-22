using Application.Dto.WorkoutPlan;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkoutTemplateMappings
    {
        public static WorkoutTemplateDto ToDto(this WorkoutTemplate template) => new(
            template.Order,
            template.Name,
            template.Exercises?.Select(e => e.ToDto()).ToList() ?? []
        );

        public static WorkoutTemplate ToEntity(this WorkoutTemplateDto dto) => new()
        {
            Order = dto.Order,
            Name = dto.Name,
            Exercises = dto.Exercises?.Select(e => e.ToEntity()).ToList() ?? []
        };
    }
}
