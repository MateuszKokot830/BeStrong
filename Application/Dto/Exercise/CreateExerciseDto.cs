using Domain.Common;

namespace Application.Dto.Exercise
{
    public record CreateExerciseDto(
        string Name,
        string? Description,
        MuscleSubgroup MuscleSubgroup,
        string? ImageUrl
    );
}
