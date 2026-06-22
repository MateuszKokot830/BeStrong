namespace Application.Dto.Comment
{
    public record CommentDto(
        int Id,
        int UserId,
        string? Description,
        DateTime CreatedDate,
        int LikesCount,
        int PostId
    );
}
