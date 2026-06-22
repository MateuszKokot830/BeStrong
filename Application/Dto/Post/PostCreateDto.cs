using Domain.Common;

namespace Application.Dto.Post
{
    public record PostCreateDto(
        PostType Type,
        string? Description,
        int? WorkoutId,
        int? WorkoutPlan
    );
}
