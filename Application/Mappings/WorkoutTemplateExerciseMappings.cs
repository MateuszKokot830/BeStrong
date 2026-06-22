using Application.Dto.WorkoutPlan;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkoutTemplateExerciseMappings
    {
        public static WorkoutTemplateExerciseDto ToDto(this WorkoutTemplateExercise exercise) => new(
            exercise.Order,
            exercise.ExerciseId
        );

        public static WorkoutTemplateExercise ToEntity(this WorkoutTemplateExerciseDto dto) => new()
        {
            Order = dto.Order,
            ExerciseId = dto.ExerciseId
        };
    }
}
