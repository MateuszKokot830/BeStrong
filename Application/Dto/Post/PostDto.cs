using Application.Dto.Comment;
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
        int LikesCount,
        IReadOnlyCollection<CommentDto> Comments
    );
}
