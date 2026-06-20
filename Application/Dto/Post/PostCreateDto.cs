namespace Application.Dto.Post
{
    public record PostCreateDto(
        string? Description,
        int? WorkoutId,
        int? WorkoutPlan
    );
}
