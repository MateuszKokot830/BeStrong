using Application.Dto.WorkoutPlan;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkoutTemplateExerciseMappings
    {
        public static WorkoutTemplateExerciseDto ToDto(this WorkoutTemplateExercise exercise) => new(
            exercise.Order,
            exercise.Exercise!.ToDto(),
            exercise.Sets,
            exercise.MinReps,
            exercise.MaxReps
        );

        public static WorkoutTemplateExercise ToEntity(this WorkoutTemplateExerciseCreateDto dto) => new()
        {
            Order = dto.Order,
            ExerciseId = dto.ExerciseId,
            Sets = dto.Sets,
            MinReps = dto.MinReps,
            MaxReps = dto.MaxReps
        };
    }
}
