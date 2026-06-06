namespace Application.Dto.Comment
{
    public record CommentCreateDto(
        int UserId,
        string? Description,
        int PostId
    );
}