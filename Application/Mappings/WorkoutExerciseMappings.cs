using Application.Dto.Workout;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkoutExerciseMappings
    {
        public static WorkoutExerciseDto ToDto(this WorkoutExercise we) => new(
            we.Sets,
            we.Reps,
            we.Weight,
            we.ExerciseId,
            we.WorkoutId
        );

        public static WorkoutExercise ToEntity(this WorkoutExerciseDto dto) => new()
        {
            Sets = dto.Sets,
            Reps = dto.Reps,
            Weight = dto.Weight,
            ExerciseId = dto.ExerciseId,
            WorkoutId = dto.WorkoutId
        };
    }
}
