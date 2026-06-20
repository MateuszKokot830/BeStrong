using Domain.Common;

namespace Application.Dto.Exercise
{
    public record ExerciseDto(
        int Id,
        string? Name,
        string? Description,
        MuscleGroup MuscleGroup,
        MuscleSubgroup MuscleSubgroup,
        string? ImageUrl
    );
}
