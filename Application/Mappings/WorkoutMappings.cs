using Application.Dto.Workout;
using Domain.Aggregates;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkoutMappings
    {
        public static Workout ToEntity(this WorkoutDto dto) => new()
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Date = dto.Date,
            Name = dto.Name,
            WorkoutExercises = dto.WorkoutExercises?.Select(we => we.ToEntity()).ToList() ?? []
        };

        public static WorkoutDto ToDto(this Workout workout) => new(
            workout.Id,
            workout.UserId,
            workout.Date,
            workout.Name,
            workout.WorkoutExercises?.Select(we => we.ToDto()).ToList() ?? []
        );

    }
}
