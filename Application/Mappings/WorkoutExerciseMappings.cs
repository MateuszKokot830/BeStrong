using Application.Dto.Workout;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkoutExerciseMappings
    {
        public static WorkoutExerciseDto ToDto(this WorkoutExercise we)
        {
            var sets = we.Sets?.Select(s => s.ToDto()).ToList() ?? [];

            return new(
                we.Order,
                we.Notes,
                we.ExerciseId,
                we.WorkoutId,
                sets.Count > 0 ? sets.Max(s => s.TotalWeight) : null,
                sets.Count > 0 ? sets.Max(s => s.EstimatedOneRepMax) : null,
                sets
            );
        }

        public static WorkoutExercise ToEntity(this WorkoutExerciseDto dto) => new()
        {
            Order = dto.Order,
            Notes = dto.Notes,
            ExerciseId = dto.ExerciseId,
            WorkoutId = dto.WorkoutId,
            Sets = dto.Sets?.Select(s => s.ToEntity()).ToList() ?? []
        };
    }
}
