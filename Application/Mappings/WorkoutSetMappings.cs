using Application.Dto.Workout;
using Domain.Entities;
using Domain.Services;

namespace Application.Mappings
{
    public static class WorkoutSetMappings
    {
        public static WorkoutSetDto ToDto(this WorkoutSet set) => new(
            set.SetNumber,
            set.Reps,
            set.Weight,
            set.Weight.HasValue ? set.Weight.Value * set.Reps : null,
            set.Weight.HasValue ? OneRepMaxCalculator.Calculate(set.Weight.Value, set.Reps) : null
        );

        public static WorkoutSet ToEntity(this WorkoutSetDto dto) => new()
        {
            SetNumber = dto.SetNumber,
            Reps = dto.Reps,
            Weight = dto.Weight
        };
    }
}
