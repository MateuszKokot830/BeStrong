namespace Application.Dto.Post
{
    public record PostCreateDto(
        int UserId,
         string? Description,
         int? WorkoutId,
         int? WorkoutPlan
    );
}