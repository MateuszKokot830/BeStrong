using Application.Dto.Comment;

namespace Application.Dto.Post
{
    public record PostDto(
        int Id,
        int UserId,
        string? Description,
        DateTime CreatedDate,
        int? WorkoutId,
        int? WorkoutPlanId,
        int Likes,
        IReadOnlyCollection<CommentDto> Comments
    );
}