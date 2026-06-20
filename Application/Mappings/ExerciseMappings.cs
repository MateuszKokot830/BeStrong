using Application.Dto.Exercise;
using Domain.Entities;

namespace Application.Mappings
{
    public static class ExerciseMappings
    {
        public static ExerciseDto ToDto(this Exercise exercise) => new(
            exercise.Id,
            exercise.Name,
            exercise.Description
        );

        public static Exercise ToEntity(this CreateExerciseDto dto) => new()
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }
}
