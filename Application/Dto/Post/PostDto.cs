using Application.Dto.Comment;
using Application.Dto.Workout;
using Domain.Common;

namespace Application.Dto.Post
{
    public record PostDto(
        int Id,
        int UserId,
        PostType Type,
        string? Description,
        DateTime CreatedDate,
        DateTime? UpdatedDate,
        int? WorkoutId,
        int? WorkoutPlanId,
        WorkoutDto? Workout,
        int LikesCount,
        IReadOnlyCollection<CommentDto> Comments
    );
}
